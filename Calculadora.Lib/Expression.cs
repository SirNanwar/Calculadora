﻿using Calculadora.Lib.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Calculadora.Lib
{
    public class Expression
    {
        public string Text;
        public double Value { get; internal set; }

        public Expression(string text)
        {
            this.Text = text;
        }


        public virtual double Resolve()
        {
            Expression operation = null;

            Regex reg;
            Match match;

            //TODOFD - somewhere in here you have to work the negatives
            //TODOFD - Also the percentages
            //TODOFD - UGLY 

            reg = new Regex(@"(.+)( mais )(.+)");
            match = reg.Match(Text);
            if (match.Success)
            {
                operation = new Add(match.Groups[1].Value, match.Groups[3].Value);
            }
            else 
            {
                //(?(?=( menos menos ))((.+?)( menos )(.+))|((.+)( menos )(.+)))
                reg = new Regex(@"(.+)( menos )(.+)");
                match = reg.Match(Text);
                if (match.Success)
                {
                    operation = new Subtract(match.Groups[1].Value, match.Groups[3].Value);
                }
                else 
                {
                    reg = new Regex(@"(.+)( a dividir por )(.+)");
                    match = reg.Match(Text);
                    if (match.Success)
                    {
                        operation = new Divide(match.Groups[1].Value, match.Groups[3].Value);
                    }
                    else 
                    {
                        reg = new Regex(@"(.+)( vezes )(.+)");
                        match = reg.Match(Text);
                        if (match.Success)
                        {
                            operation = new Multiply(match.Groups[1].Value, match.Groups[3].Value);
                        }
                        else 
                        {
                            reg = new Regex(@"(.*)(menos )(.+)");
                            match = reg.Match(Text);
                            if (match.Success)
                            {
                                operation = new Negativo(match.Groups[3].Value);
                            }
                            else 
                            {
                                reg = new Regex(@"(.+)( ponto )(.+)");
                                match = reg.Match(Text);
                                if (match.Success)
                                {
                                    operation = new DecimalLiteral(match.Groups[1].Value, match.Groups[3].Value);
                                }
                                else
                                {
                                    //In portuguese the word 'e' is used to form numbers, works as an addition
                                    reg = new Regex(@"(.+)( e )(.+)");
                                    match = reg.Match(Text);
                                    if (match.Success)
                                    {
                                        operation = new Add(match.Groups[1].Value, match.Groups[3].Value);
                                    }
                                    else
                                    {
                                        reg = new Regex(@"(.+)");
                                        match = reg.Match(Text);
                                        if (match.Success)
                                        {
                                            operation = new Literal(match.Groups[1].Value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Value = operation.Resolve();

            return Value;
        }
    }
}
