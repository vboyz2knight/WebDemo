using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ABG
/// </summary>
public class ABG
{
	public enum enABG : short { Acute_Respiratory_Acidosis, Acute_Respiratory_Alkalosis, 
                        Acute_Metabolic_Acidosis, Acute_Metabolic_Alkalosis,
                        Partly_Compensated_Respiratory_Acidosis, Partly_Compensated_Respiratory_Alkalosis, 
                        Partly_Compensated_Metabolic_Acidosis, Partly_Compensated_Metabolic_Alkalosis,
                        Compensated_Respiratory_Acidosis, Compensated_Respiratory_Alkalosis,
                        Compensated_Metabolic_Acidosis, Compensated_Metabolic_Alkalosis,
                        Unable_To_Answer_See_Mixed_Orders,Normal,Compensated_With_Unknown_Disorder    }

    public enum enStatus : short { Acidosis, Normal, Alkalosis }
   
    decimal myPH = 0;
    short myCO2 = 0;
    short myHCO3 = 0;
    short myPO2 = 0;
    public string myError = "";
    bool bErrorFree = true;

    enStatus myphAnalysis = enStatus.Normal;
    enStatus myCO2Analysis = enStatus.Normal;
    enStatus myHCO3Analysis = enStatus.Normal;

    enABG myABGAnalysis = enABG.Normal;

    public ABG(decimal PH, short CO2, short HCO3,short PO2)
    {
        myCO2 = CO2;
        myHCO3 = HCO3;
        myPH = PH;
        myPO2 = PO2;
            
    }

    private bool ValidData()
    {
        if ((myPH < 0) | (myPH > 14))
        {
            myError += " Invalid range of PH: ";
            bErrorFree = false;
        }
        else if ((myHCO3 < 0) | (myHCO3 > 200))
        {
            myError += " Invalid range of HCO3: ";
            bErrorFree = false;
        }
        else if ((myCO2 < 0) | (myCO2 > 200))
        {
            myError += " Invalid range of CO2: ";
            bErrorFree = false;
        }

        return bErrorFree;
    }

    private string OxygenCheck()
    {
        string result = "";

        if (myPO2 >= 80)
        {
            result = "Normal PO2. ";
        }
        else if( myPO2 >= 60 )
        {
            result = "Mild Hypoxemia PO2. ";
        }
        else if( myPO2 >= 40)
        {
            result = "Moderate Hypoxemia PO2. ";
        }
        else if( myPO2 <40)
        {
            result = "Sever Hypoxemia PO2. ";
        }

        return result;
    }

    public override string ToString()
    {
        string Result= "Disorder: ";

        switch (myABGAnalysis)
        {
            case enABG.Acute_Respiratory_Acidosis:
                Result += enABG.Acute_Respiratory_Acidosis.ToString();
                break;
            case enABG.Acute_Respiratory_Alkalosis:
                Result += enABG.Acute_Respiratory_Alkalosis.ToString();
                break;
            case enABG.Acute_Metabolic_Acidosis:
                Result += enABG.Acute_Metabolic_Acidosis.ToString();
                break;
            case enABG.Acute_Metabolic_Alkalosis:
                Result += enABG.Acute_Metabolic_Alkalosis.ToString();
                break;
            case enABG.Partly_Compensated_Respiratory_Acidosis:
                Result += enABG.Partly_Compensated_Respiratory_Acidosis.ToString();
                break;
            case enABG.Partly_Compensated_Respiratory_Alkalosis:
                Result += enABG.Partly_Compensated_Respiratory_Alkalosis.ToString();
                break;
            case enABG.Partly_Compensated_Metabolic_Acidosis:
                Result += enABG.Partly_Compensated_Metabolic_Acidosis.ToString();
                break;
            case enABG.Partly_Compensated_Metabolic_Alkalosis:
                Result += enABG.Partly_Compensated_Metabolic_Alkalosis.ToString();
                break;
            case enABG.Compensated_Respiratory_Acidosis:
                Result += enABG.Compensated_Respiratory_Acidosis.ToString();
                break;
            case enABG.Compensated_Respiratory_Alkalosis:
                Result += enABG.Compensated_Respiratory_Alkalosis.ToString();
                break;
            case enABG.Compensated_Metabolic_Acidosis:
                Result += enABG.Compensated_Metabolic_Acidosis.ToString();
                break;
            case enABG.Compensated_Metabolic_Alkalosis:
                Result += enABG.Compensated_Metabolic_Alkalosis.ToString();
                break;
            case enABG.Unable_To_Answer_See_Mixed_Orders:
                Result += enABG.Unable_To_Answer_See_Mixed_Orders.ToString();
                break;
            case enABG.Normal:
                Result += enABG.Normal.ToString();
                break;
            case enABG.Compensated_With_Unknown_Disorder:
                Result += enABG.Compensated_With_Unknown_Disorder.ToString();                    
                break;
            default:
                myError += " Error in ToString(): ";
                bErrorFree = false;
                break;
        }

        Result += "\n\n";
        Result += OxygenCheck();

        return Result;
    }


    public decimal MyPH
    {

        get;
        set;
    }

    public short MyCO2
    {
        get;
        set;
    }

    public short MyHCO3
    {
        get;
        set;
    }

    public bool ABGAnalysis()
    {
        if (ValidData())
        {

            if (phAnalysis() && CO2Analysis() && HCO3Analysis())
            {

                //ph Normal
                if (myphAnalysis == enStatus.Normal)
                {
                    if (myphAnalysis == myCO2Analysis)
                    {
                        if (myCO2Analysis == myHCO3Analysis)
                        {
                            //ABG is Normal
                            myABGAnalysis = enABG.Normal;
                        }

                    }
                    else
                    {
                        RespiratoryOrMetabolic();
                    }
                }
                else
                {
                    RespiratoryOrMetabolic();
                }
            }
            else
            {
                myError += " Error in ABGAnalysis(): ";
                bErrorFree = false;
            }
        }
        else
        {
            myError += " Error in ABGAnalysis():Datas not valid: ";
            bErrorFree = false;
        }

        return bErrorFree;
    }


    private void RespiratoryOrMetabolic()
    {

        if (myphAnalysis == myCO2Analysis)
        {
            //respiratory disorder
            switch (myphAnalysis)
            {
                case (enStatus.Acidosis):
                    if (myHCO3Analysis == enStatus.Acidosis)
                    {
                        //ph acid, co2 acid, hco3 acid => mix order?
                        myABGAnalysis = enABG.Unable_To_Answer_See_Mixed_Orders;
                    }
                    else if (myHCO3Analysis == enStatus.Normal)
                    {
                        //ph acid, co2 acid, hco3 normal => acute respiratory disorder acidosis
                        myABGAnalysis = enABG.Acute_Respiratory_Acidosis;
                    }
                    else if (myHCO3Analysis == enStatus.Alkalosis)
                    {
                        //ph acid, co2 acid, hco3 alka => partial compensate respiratory acidosis disorder
                        myABGAnalysis = enABG.Partly_Compensated_Respiratory_Acidosis;
                    }

                    break;
                case (enStatus.Normal):
                    if (myHCO3Analysis == enStatus.Acidosis)
                    {
                        //ph normal, co2 normal, hco3 acid => More check!!!(Fully compensate)
                        FullyCompensate();
                    }
                    else if (myHCO3Analysis == enStatus.Normal)
                    {
                        //ph normal, co2 normal, hco3 normal => Normal ABG
                        myABGAnalysis = enABG.Normal;
                    }
                    else if (myHCO3Analysis == enStatus.Alkalosis)
                    {
                        //ph normal, co2 normal, hco3 alka => more check!!!(Fully compensate)
                        FullyCompensate();
                    }

                    break;
                case (enStatus.Alkalosis):
                    if (myHCO3Analysis == enStatus.Acidosis)
                    {
                        //ph alka, co2 alka, hco3 acid => partial compensate respiratory alkalosis disorder
                        myABGAnalysis = enABG.Partly_Compensated_Respiratory_Alkalosis;
                    }
                    else if (myHCO3Analysis == enStatus.Normal)
                    {
                        //ph alka, co2 alka, hco3 normal => Acute respiratory alkalosis
                        myABGAnalysis = enABG.Acute_Respiratory_Alkalosis;
                    }
                    else if (myHCO3Analysis == enStatus.Alkalosis)
                    {
                        //ph alka, co2 alka, hco3 alka => Mix Order!!!
                        myABGAnalysis = enABG.Unable_To_Answer_See_Mixed_Orders;
                    }

                    break;
                default:
                    myError += " Error in Checking Respiratory disorder: ";
                    bErrorFree = false;
                    break;
            }
        }
        else if (myphAnalysis == myHCO3Analysis)
        {
            //metabolic disorder
            switch (myphAnalysis)
            {
                case (enStatus.Acidosis):
                    if (myCO2Analysis == enStatus.Acidosis)
                    {
                        //ph acid, co2 acid, hco3 acid => mix order?
                        myABGAnalysis = enABG.Unable_To_Answer_See_Mixed_Orders;
                    }
                    else if (myCO2Analysis == enStatus.Normal)
                    {
                        //ph acid, co2 normal, hco3 acid => acute metabolic disorder acidosis
                        myABGAnalysis = enABG.Acute_Metabolic_Acidosis;
                    }
                    else if (myCO2Analysis == enStatus.Alkalosis)
                    {
                        //ph acid, co2 alka, hco3 acid => partial compensate metabolic acidosis disorder
                        myABGAnalysis = enABG.Partly_Compensated_Metabolic_Acidosis;
                    }

                    break;
                case (enStatus.Normal):
                    if (myCO2Analysis == enStatus.Acidosis)
                    {
                        //ph normal, co2 acid, hco3 normal => More check!!!(Fully compensate)
                        FullyCompensate();
                    }
                    else if (myCO2Analysis == enStatus.Normal)
                    {
                        //ph normal, co2 normal, hco3 normal => Normal ABG
                        myABGAnalysis = enABG.Normal;
                    }
                    else if (myCO2Analysis == enStatus.Alkalosis)
                    {
                        //ph normal, co2 alka, hco3 normal => more check!!!(Fully compensate)
                        FullyCompensate();
                    }

                    break;
                case (enStatus.Alkalosis):
                    if (myCO2Analysis == enStatus.Acidosis)
                    {
                        //ph alka, co2 acid, hco3 alka => partial compensate metabolic alkalosis disorder
                        myABGAnalysis = enABG.Partly_Compensated_Metabolic_Alkalosis;
                    }
                    else if (myCO2Analysis == enStatus.Normal)
                    {
                        //ph alka, co2 normal, hco3 alka => Acute metabolic alkalosis
                        myABGAnalysis = enABG.Acute_Metabolic_Alkalosis;
                    }
                    else if (myCO2Analysis == enStatus.Alkalosis)
                    {
                        //ph alka, co2 alka, hco3 alka => Mix Orderk!!!
                        myABGAnalysis = enABG.Unable_To_Answer_See_Mixed_Orders;
                    }

                    break;
                default:
                    myError += " Error in Checking in Metabolic disorders: ";
                    bErrorFree = false;
                    break;
            }
        }
        else if (myphAnalysis == enStatus.Normal)
        {
            if (myCO2Analysis == enStatus.Acidosis)
            {                    
                if (myHCO3Analysis == enStatus.Acidosis)
                {
                    //ph normal, co2 acid, hco3 acid
                    FullyCompensate();
                }
                else if (myHCO3Analysis == enStatus.Alkalosis)
                {
                    //ph normal, co2 acid, hoce alka
                    FullyCompensate();
                }
            }
            else if (myCO2Analysis == enStatus.Alkalosis)
            {                    
                    
                if (myHCO3Analysis == enStatus.Acidosis)
                {
                    //ph normal, co2 alka, hco3 acid
                    FullyCompensate();
                }
                else if (myHCO3Analysis == enStatus.Alkalosis)
                {
                    //ph normal, co2 alka, hoc3 alka
                    FullyCompensate();
                }
            }
        }
        else if (myphAnalysis == enStatus.Acidosis)
        {
            //ph acid, co2 alka, hco3 alka
            myABGAnalysis = enABG.Unable_To_Answer_See_Mixed_Orders;

        }
        else if (myphAnalysis == enStatus.Alkalosis)
        {
            //ph alka, co2 acid, hco3 acid
            myABGAnalysis = enABG.Unable_To_Answer_See_Mixed_Orders;
        }
        else
        {
            myError += " Unexpected checks in RespiratoryOrMetabolic(): ";
            bErrorFree = false;
        }

            

            /* 
            ((myCO2Analysis == enStatus.Normal) && (myHCO3Analysis == enStatus.Normal))
        //ph either alka or acid while co2 and hco3 normal =>
        {
            myABGAnalysis = enABG.Unable_To_Answer_See_Mixed_Orders;
        }
            * */
    }

    private void FullyCompensate()
    {
        if (myPH < 7.4m)
        {
            //acid ph
            if (myCO2Analysis == enStatus.Acidosis)
            {
                //normal-acid ph, co2 acid => Fully compensate respiratory acidosis
                myABGAnalysis = enABG.Compensated_Respiratory_Acidosis;
            }
            else if (myCO2Analysis == enStatus.Alkalosis)
            {
                //normal-acid ph, co2 alka => Fully compensate metabolic acidosis
                myABGAnalysis = enABG.Compensated_Metabolic_Alkalosis;
            }
            else if (myHCO3Analysis == enStatus.Acidosis)
            {
                //normal-acid ph, hco3 acid => Fully compensate metabolic acidosis
                myABGAnalysis = enABG.Compensated_Metabolic_Acidosis;
            }
            else if (myHCO3Analysis == enStatus.Alkalosis)
            {
                //normal-acid ph, hoc3 alka => Fully compensate respiratory acidosis
                myABGAnalysis = enABG.Compensated_Metabolic_Alkalosis;
            }
        }
        else if (myPH > 7.4m)
        {
            //alka ph
            if (myCO2Analysis == enStatus.Acidosis)
            {
                //normal-alka ph, co2 acid => Fully compensate metabolic alkalosis
                myABGAnalysis = enABG.Compensated_Metabolic_Alkalosis;
            }
            else if (myCO2Analysis == enStatus.Alkalosis)
            {
                //normal-alka ph, co2 alka => Fully compensate respiratory alkalosis
                myABGAnalysis = enABG.Compensated_Respiratory_Alkalosis;
            }
            else if (myHCO3Analysis == enStatus.Acidosis)
            {
                //normal-alka ph, hco3 acid => Fully compensate respiratory alkalosis
                myABGAnalysis = enABG.Compensated_Respiratory_Alkalosis;
            }
            else if (myHCO3Analysis == enStatus.Alkalosis)
            {
                //normal-alka ph, hoc3 alka => Fully compensate metabolic alkalosis
                myABGAnalysis = enABG.Compensated_Metabolic_Alkalosis;
            }
        }
        else if (myPH == 7.4m)
        {
            //absolute ph, co2 acid, hoc3 acid
            //absolute ph, co2 acid, hoc3 alka
            //absolute ph, co2 alka, hoc3 acid
            //absolute ph, co2 alka, hoc3 alka
            myABGAnalysis = enABG.Compensated_With_Unknown_Disorder;
                
        }
        else
        {
            myError += " Error in FullyCompensate(): ";
            bErrorFree = false;
        }
    }

    private bool phAnalysis()
    {

        if (myPH < 7.35m)
        {
            //result = "Acidosis";
            myphAnalysis = enStatus.Acidosis;
        }
        else if (myPH > 7.45m)
        {
            //result = "Alkalosis";
            myphAnalysis = enStatus.Alkalosis;
        }
        else if ((myPH >= 7.35m) && (myPH <= 7.45m))
        {
            //result = "Normal";
            myphAnalysis = enStatus.Normal;
        }
        else
        {
            myError += " Error in phAnalysis(): ";
            bErrorFree = false;
        }

        return bErrorFree;
    }

    private bool CO2Analysis()
    {

        if (myCO2 > 45)
        {
            //result = "Acidosis";
            myCO2Analysis = enStatus.Acidosis;
        }
        else if (myCO2 < 35)
        {
            //result = "Alkalosis";
            myCO2Analysis = enStatus.Alkalosis;
        }
        else if ((myCO2 >= 35) && (myCO2 <= 45))
        {
            //result = "Normal";
            myCO2Analysis = enStatus.Normal;
        }
        else
        {
            myError += " Error in CO2Analysis(): ";
            bErrorFree = false;
        }

        return bErrorFree;
    }

    private bool HCO3Analysis()
    {

        if (myHCO3 < 22)
        {
            //result = "Acidosis";
            myHCO3Analysis = enStatus.Acidosis;
        }
        else if (myHCO3 > 26)
        {
            // result = "Alkalosis";
            myHCO3Analysis = enStatus.Alkalosis;
        }
        else if ((myHCO3 >= 22) && (myHCO3 <= 26))
        {
            //result = "Normal";
            myHCO3Analysis = enStatus.Normal;
        }
        else
        {
            myError += " Error in HCO3Analysis(): ";
            bErrorFree = false;
        }

        return bErrorFree;
    }
  
}