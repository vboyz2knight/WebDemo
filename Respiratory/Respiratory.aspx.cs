using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Collections;


public partial class Respiratory : System.Web.UI.Page
{
        string mainEquation = "";
        Stack<string> mainStack = new Stack<string>();

        public enum enOperator : short
        {
            add, minus, multiply, divide
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string calc_query = Page.Request.QueryString["id"];
                RespiratoryCalc.Visible = true;

                if(calc_query.Equals("analyzerABG") )
                {
                    BuildABGquery();
                }
                else if( calc_query.Equals("VentilatorAssistant") )
                {
                    BuildVentilatorAssistant();
                }
                else
                {
                    BuildCalculationQueries(calc_query);
                }
            }
            else
            {
                RespiratoryCalc.Visible = false;
            }
        }

        private string PutInputIntoEquation(string tmpInput,string InfixExpression)
        {
            double doubleResult = 0.00f;
            ContentPlaceHolder cph = (ContentPlaceHolder)Master.FindControl("MainContent");

            if (cph != null)
            {
                var holder = cph.FindControl("PlaceHolderEquation");

                if (holder != null)
                {
                    var tb = (TextBox)holder.FindControl(tmpInput);
                    if (tb != null)
                    {
                        if (double.TryParse(tb.Text, out doubleResult))
                        {
                            InfixExpression = InfixExpression.Replace(tmpInput, tb.Text);
                            tmpInput = ""; //reset variable name
                        }
                        else
                        {
                            //error unable to find input, equation can't be evaluate
                            lblError.Text = "Unable to parse input.";
                        }
                    }
                    else
                    {
                        //error unable to find input, equation can't be evaluate
                        lblError.Text = "Unable to find input.";
                    }
                }
                else
                {
                    //error unable to find input, equation can't be evaluate
                    lblError.Text = "Unable to find PlaceHolderEquation.";
                }
            }
            else
            {
                //error unable to find input, equation can't be evaluate
                lblError.Text = "Unable to find MainContent.";
            }

            return InfixExpression;
        }

        private bool IsVariable(string tmp)
        {
            bool bReturn = false;
            double number = 0.00f;

            bReturn = !double.TryParse(tmp,out number);

            return bReturn;
        }

        private void GiveAnswer(object sender, EventArgs e)
        {
            string inputValues = "";
            string tmpInput = "";

            Stack<string> inputStack = new Stack<string>();
            //inputStack = mainStack;

            foreach (string value in mainStack)
            {
                inputValues += value + "::";
            }

            
            //
            mainEquation.Trim();
            char[] equation = mainEquation.ToArray();
            string InfixExpression = mainEquation;

            //replacing variables with user input
            for (int i = 0; i < equation.Length; i++)
            {
                switch (equation[i])
                {
                    case '/':                        
                    case '+':
                    case '-':
                    case '*':
                        if (tmpInput.Length > 0)
                        {
                            InfixExpression = PutInputIntoEquation(tmpInput,InfixExpression);
                            tmpInput = "";
                        }
                        break;
                    default:
                        if ((equation[i] != '(') && (equation[i] != ')'))
                        {
                            tmpInput += equation[i];
                        }
                        break;
                }
            }

            if (tmpInput.Length > 0)
            {
                if (IsVariable(tmpInput))
                {
                    InfixExpression = PutInputIntoEquation(tmpInput, InfixExpression);
                }
            }
            //

            ContentPlaceHolder cph2 = (ContentPlaceHolder)Master.FindControl("MainContent");

            if (cph2 != null)
            {
                var holder = cph2.FindControl("PlaceHolderEquation");

                if (holder != null)
                {
                    var tb = (TextBox)holder.FindControl("Answer");
                    if (tb != null)
                    {
                        SimpleExpressionParser myExpressionParser = new SimpleExpressionParser(InfixExpression);
                        tb.Text = myExpressionParser.myAnswer;
                    }
                    else
                    {
                        //error unable to find input, equation can't be evaluate
                        lblError.Text = "Unable to find Answer textbox.";
                    }
                }
                else
                {
                    //error unable to find input, equation can't be evaluate
                    lblError.Text = "Unable to find PlaceHolderEquation.";
                }
            }
            else
            {
                //error unable to find input, equation can't be evaluate
                lblError.Text = "Unable to find MainContent.";
            }
        }

        private void GiveABGanswer(object sender, EventArgs e)
        {
            ContentPlaceHolder cph2 = (ContentPlaceHolder)Master.FindControl("MainContent");

            if (cph2 != null)
            {
                var holder = cph2.FindControl("PlaceHolderEquation");

                if (holder != null)
                {
                    var tb = (TextBox)holder.FindControl("Answer");
                    var ph = (TextBox)holder.FindControl("ph");
                    var pao2 = (TextBox)holder.FindControl("pao2");
                    var co2 = (TextBox)holder.FindControl("co2");
                    var hco3 = (TextBox)holder.FindControl("hco3");
                    if (tb != null && ph!=null&&pao2!=null&&co2!=null&&hco3!=null)
                    {
                        decimal i_ph=0;
                        short i_pao2, i_co2, i_hco3 = 0;
                        try
                        {
                            decimal.TryParse(ph.Text,out i_ph);
                            short.TryParse(pao2.Text, out i_pao2);
                            short.TryParse(co2.Text, out i_co2);
                            short.TryParse(hco3.Text, out i_hco3);

                            ABG myABG = new ABG(i_ph, i_co2, i_hco3, i_pao2);

                            if (myABG.ABGAnalysis())
                            {
                                tb.Text = myABG.ToString();
                            }
                            else
                            {
                                tb.Text = "Error";
                            }
                        }
                        catch
                        {
                            tb.Text = "Unable to parse inputs.";
                        }
                        
                    }
                    else
                    {
                        //error unable to find input, equation can't be evaluate
                        lblError.Text = "Unable to find input textboxes.";
                    }
                }
                else
                {
                    //error unable to find input, equation can't be evaluate
                    lblError.Text = "Unable to find PlaceHolderEquation.";
                }
            }
            else
            {
                //error unable to find input, equation can't be evaluate
                lblError.Text = "Unable to find Answer textbox.";
            }
        }
        private void BuildABGquery()
        {
            PlaceHolderEquation.Controls.Clear();
            Table tbl = new Table();            

            TableRow r = new TableRow();
            TableCell c = new TableCell();
            c.Controls.Add(new LiteralControl("ABG Analyzer"));
            c.HorizontalAlign = HorizontalAlign.Center;
            
            r.Cells.Add(c);
            tbl.Rows.Add(r);
            tbl.Rows[0].Cells[0].ColumnSpan = 3;

            BuildABGRow(tbl, "ph", 15);
            BuildABGRow(tbl, "co2", 15);
            BuildABGRow(tbl, "hco3", 15);
            BuildABGRow(tbl, "pao2", 15);

            //submit button
            TableRow submitRow = new TableRow();
            TableCell answerCell = new TableCell();
            TextBox answerTxtBox = new TextBox();
            answerTxtBox.ID = "Answer";
            answerTxtBox.ReadOnly = true;
            answerTxtBox.Width = 300;
            answerTxtBox.TextMode = TextBoxMode.MultiLine;
            answerTxtBox.Rows = 5;
            answerCell.ColumnSpan = 3;
            answerCell.Wrap = true;

            TableCell submitCell = new TableCell();
            Button submitButton = new Button();
            submitButton.Text = "Analyze!";
            submitButton.ID = "Submit";
            submitButton.Click += new System.EventHandler(this.GiveABGanswer);

            answerCell.Controls.Add(answerTxtBox);
            submitCell.Controls.Add(submitButton);
            submitRow.Cells.Add(submitCell);
            submitRow.Cells.Add(answerCell);
            tbl.Rows.Add(submitRow);            
        
            PlaceHolderEquation.Controls.Add(tbl);
        }
        private void BuildABGRow(Table tbl,string name,int txtBoxWidth)
        {
            TableRow row = new TableRow();
            TableCell cell1 = new TableCell();
            TableCell cell2 = new TableCell();
            TableCell cell3 = new TableCell();

            cell1.Controls.Add(new LiteralControl(name));
            TextBox txtBox = new TextBox();
            txtBox.ID = name;
            txtBox.Width = 100;
            txtBox.MaxLength = 10;
            
            cell2.Controls.Add(txtBox);

            switch (name)
	        {
                case "ph":  
                    RangeValidator rv = new RangeValidator();
                    rv.ControlToValidate = txtBox.ID;
                    rv.Type = ValidationDataType.Currency;

                    rv.MinimumValue = "0";
                    rv.MaximumValue = "14.00";
                    rv.ErrorMessage = "0.00-14.00";
                  
                    cell3.Controls.Add(rv);
                    break;
                case "co2":
                    RangeValidator rvco2 = new RangeValidator();
                    rvco2.ControlToValidate = txtBox.ID;
                    rvco2.Type = ValidationDataType.Currency;
                    rvco2.MinimumValue = "0";
                    rvco2.MaximumValue = "200.00";
                    rvco2.ErrorMessage = "0.00-200.00";
                    cell3.Controls.Add(rvco2);
                    break;
                case "hco3":
                    RangeValidator rvhco3 = new RangeValidator();
                    rvhco3.ControlToValidate = txtBox.ID;
                    rvhco3.Type = ValidationDataType.Currency;
                    rvhco3.MinimumValue = "0";
                    rvhco3.MaximumValue = "100.00";
                    rvhco3.ErrorMessage = "0.00-100.00";
                    cell3.Controls.Add(rvhco3);
                    break;
                case "pao2":
                    RangeValidator rvpao2 = new RangeValidator();
                    rvpao2.ControlToValidate = txtBox.ID;
                    rvpao2.Type = ValidationDataType.Currency;
                    rvpao2.MinimumValue = "0";
                    rvpao2.MaximumValue = "600.00";
                    rvpao2.ErrorMessage = "0.00-600.00";
                    cell3.Controls.Add(rvpao2);
                    break;
	        }

            RequiredFieldValidator rfv = new RequiredFieldValidator();
            rfv.ControlToValidate = txtBox.ID;
            rfv.ErrorMessage = "Empty Input";
            cell3.Controls.Add(rfv);

            row.Cells.Add(cell1);
            row.Cells.Add(cell2);
            row.Cells.Add(cell3);
            tbl.Rows.Add(row);
        }

        private void BuildVentilatorAssistant()
        {
            PlaceHolderEquation.Controls.Clear();
            Table tbl = new Table();

            TableRow row = new TableRow();

            TableCell cell = new TableCell();
            cell.Controls.Add(new LiteralControl("Gender "));
            DropDownList genderDropDownList = new DropDownList();
            genderDropDownList.Items.Add(new ListItem("Male") );
            genderDropDownList.Items.Add(new ListItem("Female"));
            cell.Controls.Add(genderDropDownList);
            row.Cells.Add(cell);            

            TableCell cell2 = new TableCell();
            cell2.Controls.Add(new LiteralControl("Age "));
            TextBox ageTxtBox = new TextBox();
            ageTxtBox.Width = 50;
            ageTxtBox.MaxLength = 3;
            cell2.Controls.Add(ageTxtBox);
            row.Cells.Add(cell2);

            TableCell cell3 = new TableCell();
            cell3.Controls.Add(new LiteralControl("Settings "));
            DropDownList settingDropDownList = new DropDownList();
            settingDropDownList.Items.Add(new ListItem("Initial Vent Settings"));
            settingDropDownList.Items.Add(new ListItem("Exist Vent Settings with ABG"));
            cell3.Controls.Add(settingDropDownList);
            row.Cells.Add(cell3);

            tbl.Rows.Add(row);

            

            PlaceHolderEquation.Controls.Add(tbl);
        }

        private void BuildCalculationQueries(string calc_query)
        {
            XDocument sqlBasic = XDocument.Load(Server.MapPath("RespiratoryEquations.xml"));
            var sqls = from _sql in sqlBasic.Descendants("resp")
                       where _sql.Element("abbreviation").Value == calc_query
                       select new
                       {
                           Command = _sql.Element("abbreviation"),
                           Descriptions = _sql.Element("description"),
                           ID = _sql.Element("id"),
                           Equations = (from l in _sql.Descendants("equation")
                                        select new
                                        {
                                            example = l.Value

                                        })
                       };

            foreach (var p in sqls)
            {
                //build my equation table
                PlaceHolderEquation.Controls.Clear();
                Table tbl = new Table();

                foreach (var q in p.Equations)
                {
                    string equation = q.example;
                    mainEquation = equation;

                    //take out the white space in the equation
                    equation = equation.Replace(" ", "");
                    char[] equationChar = equation.ToArray();
                    string tmpInput = "";
                    double doubleResult = 0.000;

                    TableRow r = new TableRow();
                    TableCell c = new TableCell();
                    c.Controls.Add(new LiteralControl(p.Command.ToString() + " = " + q.example.ToString()));
                    r.Cells.Add(c);
                    tbl.Rows.Add(r);
                    tbl.Rows[0].Cells[0].ColumnSpan = 3;

                    TableRow r2 = new TableRow();
                    TableCell c2 = new TableCell();
                    c2.Controls.Add(new LiteralControl(p.Command.ToString() + " : " + p.ID.ToString() + " : " + p.Descriptions.ToString()));
                    r2.Cells.Add(c2);
                    tbl.Rows.Add(r2);
                    tbl.Rows[1].Cells[0].ColumnSpan = 3;

                    Stack<string> inputStack = new Stack<string>();
                    //finding all variables in the euqation
                    //look at the equation each characters to see if it not /,+,-,*,(,)
                    //we keep adding the char until one of those match then we put it in a stack.
                    for (int i = 0; i < equationChar.Length; i++)
                    {
                        switch (equationChar[i])
                        {
                            case '/':
                            case '+':
                            case '-':
                            case '*':
                            case '(':
                            case ')':
                                if (tmpInput.Length > 0)
                                {
                                    //if it a number don't use it
                                    if (!double.TryParse(tmpInput, out doubleResult))
                                    {
                                        //if it's duplicate don't use it
                                        if (!inputStack.Contains(tmpInput))
                                        {
                                            inputStack.Push(tmpInput);
                                            mainStack.Push(tmpInput);
                                        }
                                    }
                                }

                                tmpInput = "";
                                break;
                            default:
                                tmpInput += equationChar[i];
                                break;
                        }
                    }

                    //use the stack to ask for all variables
                    foreach (string value in inputStack)
                    {
                        TableRow row = new TableRow();
                        TableCell Cell1 = new TableCell();
                        TableCell Cell2 = new TableCell();
                        TableCell Cell3 = new TableCell();

                        Cell1.Controls.Add(new LiteralControl(value));

                        TextBox txtBox = new TextBox();
                        txtBox.ID = value;
                        txtBox.Width = 50;
                        Cell2.Controls.Add(txtBox);

                        RequiredFieldValidator rfv = new RequiredFieldValidator();
                        rfv.ControlToValidate = txtBox.ID;
                        rfv.ErrorMessage = "Empty Input";
                        Cell3.Controls.Add(rfv);

                        RangeValidator rv = new RangeValidator();
                        rv.ControlToValidate = txtBox.ID;
                        rv.Type = ValidationDataType.Currency;
                        rv.MinimumValue = "0";
                        rv.MaximumValue = "1000";
                        rv.ErrorMessage = "0.00-1000.00";
                        Cell3.Controls.Add(rv);

                        row.Cells.Add(Cell1);
                        row.Cells.Add(Cell2);
                        row.Cells.Add(Cell3);
                        tbl.Rows.Add(row);
                    }

                    //submit button
                    TableRow submitRow = new TableRow();
                    TableCell answerCell = new TableCell();
                    TextBox answerTxtBox = new TextBox();
                    answerTxtBox.ID = "Answer";
                    answerTxtBox.ReadOnly = true;
                    answerTxtBox.Width = 100;

                    TableCell submitCell = new TableCell();
                    Button submitButton = new Button();
                    submitButton.Text = "Calculate!";
                    submitButton.ID = "Submit";
                    submitButton.Click += new System.EventHandler(this.GiveAnswer);

                    answerCell.Controls.Add(answerTxtBox);
                    submitCell.Controls.Add(submitButton);
                    submitRow.Cells.Add(submitCell);
                    submitRow.Cells.Add(answerCell);
                    tbl.Rows.Add(submitRow);

                    inputStack.Clear();
                }
                PlaceHolderEquation.Controls.Add(tbl);
            }
        }
}