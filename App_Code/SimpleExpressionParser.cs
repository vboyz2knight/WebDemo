using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//only numbers and + - * / ( ) are allows
public class SimpleExpressionParser
    {
        private string InfixExpression = "";
        private string PostfixExpression = "";
        private string Answer = "";

        public string myAnswer
        {
            get
            {
                return Answer;
            }
        }

        public SimpleExpressionParser(string expression)
        {
            if (expression.Length > 0)
            {
                InfixExpression = expression;
                Eval();
            }
        }

        public string Eval()
        {
            string answer = "";

            if (IsValidData())
            {
                Answer = SolveMe();
            }
            else
            {
                Answer = "Unsupported data in expression.  Unable to evaluate expression.";
            }

            return answer;
        }

        private string SolveMe()
        {

            toPostFixOrder();
            PostFixEval();

            return Answer;
        }
        /*
            1. read postfix expression token by token
            2.   if the token is an operand, push it 
                 into the stack
            3.   if the token is a binary operator, 
            3.1    pop the two top most operands 
                   from the stack
            3.2    apply the binary operator with the 
                   two operands
            3.3    push the result into the stack
            4. finally, the value of the whole postfix 
               expression remains in the stack
         * */
        private string PostFixEval()
        {
            /*
            Initialize(Stack S)
            x = ReadToken();  // Read Token
            while(x)
            {
                    if ( x is Operand )
                        Push ( x ) Onto Stack S.

                    if ( x is Operator )
                        {
                        Operand2 = Pop(Stack S);
                        Operand2 = Pop(Stack S);
                        Evaluate (Operand1,Operand2,Operator x);
                        ??Push answer back to stack s??
                        }

                    x = ReadNextToken();  // Read Token
            }
             * */

            Stack<double> operandStack = new Stack<double>();
            string readInput = readNextInput();
            string myAnswer = "";
            bool bError = false;
            double dAnswer = 0.00f;

            while (readInput.Length > 0)
            {
                if (bError)
                {
                    break;
                }

                if (IsOperand(readInput))
                {
                    double outDouble = 0.00;

                    if (double.TryParse(readInput, out outDouble))
                    {
                        operandStack.Push(outDouble);
                    }
                    else
                    {
                        myAnswer = "Error unable to parse double: " + readInput;
                        bError = true;
                    }
                }
                else if (readInput.Length == 1)
                {
                    char[] tmp = readInput.ToCharArray();

                    if (isOperator(tmp[0]))
                    {
                        if (operandStack.Count >= 2)
                        {
                            double operand2 = operandStack.Pop();
                            double operand1 = operandStack.Pop();

                            dAnswer = Evaluate(operand1, operand2, tmp[0]);

                            operandStack.Push(dAnswer);
                        }
                        else
                        {
                            myAnswer = "Trying to evaluate with less than 2 operand in stack.";

                        }
                    }
                    else
                    {
                        myAnswer = "Error unable to parse operator: " + readInput;
                        bError = true;
                    }
                }

                readInput = readNextInput();
            }

            if (operandStack.Count == 1)
            {
                Answer = operandStack.Pop().ToString();
            }

            return myAnswer;
        }
        private double Evaluate(double operand1, double operand2, char p)
        {
            double myEAnswer = 0.00f;

            switch (p)
            {
                case '/':
                    if (operand2 != 0)
                    {
                        myEAnswer = operand1 / operand2;
                    }
                    else
                    {
                        myEAnswer = -1;
                    }
                    break;
                case '+':
                    myEAnswer = operand1 + operand2;
                    break;
                case '-':
                    myEAnswer = operand1 - operand2;
                    break;
                case '*':
                    myEAnswer = operand1 * operand2;
                    break;
                default:
                    myEAnswer = -1;
                    break;
            }

            return myEAnswer;
        }

        private string readNextInput()
        {
            bool bFirstInputFound = false;
            string myAnswer = "";
            string newPostFixExpression = "";
            char[] tmpPostFixExpression = PostfixExpression.ToCharArray();

            for (int i = 0; i < tmpPostFixExpression.Length; i++)
            {
                if (!bFirstInputFound)
                {
                    if (tmpPostFixExpression[i] != '|' && !isOperator(tmpPostFixExpression[i]))
                    {
                        myAnswer += tmpPostFixExpression[i];
                    }
                    else if (isOperator(tmpPostFixExpression[i]))
                    {
                        myAnswer = tmpPostFixExpression[i].ToString();
                        bFirstInputFound = true;
                    }
                    else if (tmpPostFixExpression[i] == '|')
                    {
                        bFirstInputFound = true;
                    }
                }
                else
                {
                    newPostFixExpression += tmpPostFixExpression[i];
                }
            }

            PostfixExpression = newPostFixExpression;

            return myAnswer;
        }
        private short PrecedenceNum(char top)
        {
            switch (top)
            {
                case '/':
                    return 4;
                case '*':
                    return 3;
                case '+':
                    return 2;
                case '-':
                    return 1;
                default:
                    return -1;
            }
        }

        /*
         1. Scan the Infix Expression from left to right.
         2. If the scannned character is an operand, copy it to the Postfix Expression.
         3. If the scanned character is left parentheses, push it onto the stack.
         4. If the scanned character is right parenthesis, the symbol at the top of the stack is popped off the stack and copied to the Postfix Expression. Repeat until the symbol at the top of the stack is a left parenthesis (both parentheses are discarded in this process).
         5. If the scanned character is an operator and has a higher precedence than the symbol at the top of the stack, push it onto the stack.
         6. If the scanned character is an operator and the precedence is lower than or equal to the precedence of the operator at the top of the stack, one element of the stack is popped to the Postfix Expression; repeat this step with the new top element on the stack. Finally, push the scanned character onto the stack.
         7. After all characters are scanned, the stack is popped to the Postfix Expression until the stack is empty
         * */
        private void toPostFixOrder()
        {
            string tmpInput = "";
            Stack<char> operator_stack = new Stack<char>();
            string PostFix = "";

            string tmpInfix = InfixExpression;

            tmpInfix = tmpInfix.Insert(0, "(");
            tmpInfix = tmpInfix.Insert(tmpInfix.Length, ")");

            char[] infixExpression = tmpInfix.ToCharArray();

            for (int i = 0; i < infixExpression.Length; i++)
            {
                if (isOperator(infixExpression[i]))
                {
                    //we found an operator implying previous tmpInput as Operand?
                    if (IsOperand(tmpInput))
                    {
                        //insert | to tell us that this seperate each input because we scanning one char
                        //at a time but we can have one input as decimal
                        PostFix += tmpInput + "|";

                        tmpInput = "";
                    }

                    if (operator_stack.Count == 0)
                    {
                        operator_stack.Push(infixExpression[i]);
                    }
                    else
                    {
                        while (PrecedenceNum(operator_stack.Peek()) >= PrecedenceNum(infixExpression[i]))
                        {
                            PostFix += operator_stack.Pop();
                        }

                        operator_stack.Push(infixExpression[i]);
                        
                    }

                }
                else if (infixExpression[i] == '(')
                {
                    operator_stack.Push(infixExpression[i]);
                }
                else if (infixExpression[i] == ')')
                {
                    ////////////////////////
                    if (tmpInput.Length > 0)
                    {
                        PostFix += tmpInput + "|";
                        tmpInput = "";
                    }

                    while (operator_stack.Count > 0 && operator_stack.Peek() != '(')
                    {
                        PostFix += operator_stack.Pop();
                    }

                    //pop the left parentheses
                    operator_stack.Pop();
                }
                else
                {
                    tmpInput += infixExpression[i];
                }
            }

            while (operator_stack.Count != 0)
            {
                PostFix += operator_stack.Pop();
            }

            PostfixExpression = PostFix;
            Answer = PostFix;
        }

        private bool IsOperand(string tmpInput)
        {
            bool bReturn = false;
            double outDouble = 0.00;

            if (double.TryParse(tmpInput, out outDouble))
            {
                bReturn = true;
            }


            return bReturn;
        }

        private bool isOperator(char p)
        {
            bool bReturn = false;

            switch (p)
            {
                case '/':
                case '+':
                case '-':
                case '*':
                    bReturn = true;
                    break;
                default:
                    bReturn = false;
                    break;
            }

            return bReturn;
        }

        private bool IsValidData()
        {
            bool bValid = true;
            string tmpInput = "";
            double doubleResult = 0.00f;

            if (IsMatchingParentheses())
            {
                //look at the equation for each character to see if it not /,+,-,*,(,), number
                for (int i = 0; i < InfixExpression.Length; i++)
                {
                    switch (InfixExpression[i])
                    {
                        case '/':
                        case '+':
                        case '-':
                        case '*':
                        case '(':
                        case ')':
                            if (tmpInput.Length > 0)
                            {
                                //if it not a number then you have unsupported data in expression
                                if (!double.TryParse(tmpInput, out doubleResult))
                                {
                                    bValid = false;
                                }
                            }

                            tmpInput = "";
                            break;
                        default:
                            tmpInput += InfixExpression[i];
                            break;
                    }
                }
            }

            return bValid;
        }
        private bool IsMatchingParentheses()
        {
            Stack<char> tempStack = new Stack<char>();
            bool bMatching = true;
            bool bReturn = false;
            int index = 0;

            //no parentheses, mean matching parentheses
            if (!InfixExpression.Contains('(') && !InfixExpression.Contains(')'))
            {
                bReturn = true;
            }
            else
            {
                //numbers of ( and ) does not match mean no matching parentheses
                //int openParentheses = InfixExpression.Count(x => x == '(');
                //int closeParentheses = InfixExpression.Count(x => x == ')');
                int openParentheses = 0;
                int closeParentheses = 0;
                string stringParentheses = "";

                for (int i = 0; i < InfixExpression.Length; i++)
                {
                    if (InfixExpression[i] == '(')
                    {
                        openParentheses++;
                        stringParentheses += '(';
                    }
                    else if (InfixExpression[i] == '(')
                    {
                        closeParentheses++;
                        stringParentheses += ')';
                    }
                }

                if (openParentheses != closeParentheses)
                {
                    bMatching = false;
                }
                else
                {
                    while ((index < stringParentheses.Length) && bMatching)
                    {

                        if (stringParentheses[index].Equals('('))
                        {
                            tempStack.Push('(');
                        }
                        else
                        {
                            if (tempStack.Count == 0)
                            {
                                bMatching = false;
                            }
                            else
                            {
                                tempStack.Pop();
                            }
                        }
                        index++;
                    }
                }
            }

            if ((tempStack.Count == 0) && bMatching)
            {
                bReturn = true;
            }

            return bReturn;
        }

    }

