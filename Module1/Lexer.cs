using System;
using System.Collections.Generic;

public class LexerException : System.Exception
{
    public LexerException(string msg)
        : base(msg)
    {
    }

}

public class Lexer
{

    protected int position;
    protected char currentCh;       // очередной считанный символ
    protected int currentCharValue; // целое значение очередного считанного символа
    protected System.IO.StringReader inputReader;
    protected string inputString;

    public Lexer(string input)
    {
        inputReader = new System.IO.StringReader(input);
        inputString = input;
    }

    public void Error()
    {
        System.Text.StringBuilder o = new System.Text.StringBuilder();
        o.Append(inputString + '\n');
        o.Append(new System.String(' ', position - 1) + "^\n");
        o.AppendFormat("Error in symbol {0}", currentCh);
        throw new LexerException(o.ToString());
    }

    protected void NextCh()
    {
        this.currentCharValue = this.inputReader.Read();
        this.currentCh = (char)currentCharValue;
        this.position += 1;
    }

    public virtual void Parse()
    {

    }
    public virtual int Save()
    {
        return 0;
    }

    public virtual void SaveList()
    {
    }

    public virtual string SaveStr()
    {
        return "";
    }

    public virtual double SaveDouble()
    {
        return 0;
    }
}

public class IntLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IntLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Integer is recognized");

    }

    public override int Save()// Задание 1
    {
        NextCh();
        intString = new System.Text.StringBuilder();
        if (currentCh == '+' || currentCh == '-')
        {
            intString.Append(currentCh);
            NextCh();

        }

        if (char.IsDigit(currentCh))
        {
            intString.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            intString.Append(currentCh);
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
            return 0;
        }

        System.Console.WriteLine("Integer is recognized");
        int res = 0;
        int ind = 0;
        if (!char.IsDigit(intString[0]))
            ind = 1;
        for (int i = ind; i < intString.Length; i++)
        {
            int x = intString[i] - 48;
            res = res * 10 + x;
        }
        if (intString[0] == '-')
            res *= -1;
        return res;
    }
}

public class Identificator: Lexer //Задание 2
{
    protected System.Text.StringBuilder idString;

    public Identificator(string input)
        : base(input)
    {
        idString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }
        

        while (char.IsLetterOrDigit(currentCh))
        {
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Identificator is recognized");

    }

}

public class IntStartsNot0 : IntLexer //Задание 3
{

    public IntStartsNot0(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            NextCh();
        }

        if (char.IsDigit(currentCh) && (currentCh != '0'))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("IntStartsNot0 is recognized");

    }
    
}


public class LetDig : Lexer //Задание 4
{
    protected System.Text.StringBuilder idString;

    public LetDig(string input)
        : base(input)
    {
        idString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }


        while (char.IsDigit(currentCh))
        {
            NextCh();
            if (currentCharValue == -1)
                break;
            if (char.IsLetter(currentCh))
            {
                NextCh();
            }
            else
            {
                Error();
            }
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Letter/Digit is recognized");

    }

}


public class StrWithMarks : Lexer // Задание 5
{
    protected System.Text.StringBuilder intString;

    public StrWithMarks(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsLetter(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsLetter(currentCh) || (currentCh == ',') || (currentCh == ';'))
        {
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("String with punctation marks is recognized");

    }

    public override void SaveList()
    {
        NextCh();

        List<string> res = new List<string>();

        System.Text.StringBuilder curstr = new System.Text.StringBuilder();

        if (char.IsLetter(currentCh))
        {
            curstr.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsLetter(currentCh) || (currentCh == ',') || (currentCh == ';'))
        {
            if (char.IsLetter(currentCh))
                curstr.Append(currentCh);
            else
            {
                res.Add(curstr.ToString());
                curstr.Clear();
            }
            NextCh();
        }

        if (curstr.Length != 0)
        {
            res.Add(curstr.ToString());
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("String with punctation marks is recognized");

        for (int i = 0; i < res.Count; i++)
        {
            System.Console.Write(res[i] + "/");
        }
        System.Console.WriteLine();
    }
}


public class StrWithSpaces : Lexer // Доп задание 1
{
    protected System.Text.StringBuilder intString;

    public StrWithSpaces(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsLetter(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsLetter(currentCh) || (currentCh == ' '))
        {
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("String with spaces is recognized");

    }

    public override void SaveList()
    {
        NextCh();

        List<string> res = new List<string>();

        System.Text.StringBuilder curstr = new System.Text.StringBuilder();

        if (char.IsLetter(currentCh))
        {
            curstr.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsLetter(currentCh) || (currentCh == ' '))
        {
            if (char.IsLetter(currentCh))
                curstr.Append(currentCh);
            else
            {
                if (curstr.Length != 0)
                {
                    res.Add(curstr.ToString());
                    curstr.Clear();
                }
            }
            NextCh();
        }
        
        if (curstr.Length != 0)
        {
            res.Add(curstr.ToString());
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
            
        }

        System.Console.WriteLine("String with spaces is recognized");

        for (int i = 0; i < res.Count; i++)
        {
            System.Console.Write(res[i] + "/");
        }
        System.Console.WriteLine();
    }
}


public class GroupLetDig : Lexer // Доп задание 2
{
    protected System.Text.StringBuilder intString;

    public GroupLetDig(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        System.Text.StringBuilder curstr = new System.Text.StringBuilder();
        while (char.IsLetterOrDigit(currentCh))
        {
            if (char.IsLetter(currentCh))
            {
                if (char.IsLetter(curstr[0]))
                {
                    curstr.Append(currentCh);
                    if (curstr.Length > 2)
                        Error();
                }
                else
                {
                    curstr.Clear();
                    curstr.Append(currentCh);
                }
            }
            if (char.IsDigit(currentCh))
            {
                if (char.IsDigit(curstr[0]))
                {
                    curstr.Append(currentCh);
                    if (curstr.Length > 2)
                        Error();
                }
                else
                {
                    curstr.Clear();
                    curstr.Append(currentCh);
                }
            }
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Group of letters and digits is recognized");

    }

    public override string SaveStr()
    {
        NextCh();

        System.Text.StringBuilder res = new System.Text.StringBuilder();
        System.Text.StringBuilder curstr = new System.Text.StringBuilder();

        if (char.IsLetterOrDigit(currentCh))
        {
            curstr.Append(currentCh);
            NextCh();
        }
        else
            Error();

        while (char.IsLetterOrDigit(currentCh))
        {
            if (char.IsLetter(currentCh))
            {
                if (char.IsLetter(curstr[0]))
                {
                    curstr.Append(currentCh);
                    if (curstr.Length > 2)
                        Error();
                }
                else
                {
                    res.Append(curstr);
                    curstr.Clear();
                    curstr.Append(currentCh);
                }
            }
            if (char.IsDigit(currentCh))
            {
                if (char.IsDigit(curstr[0]))
                {
                    curstr.Append(currentCh);
                    if (curstr.Length > 2)
                        Error();
                }
                else
                {
                    res.Append(curstr);
                    curstr.Clear();
                    curstr.Append(currentCh);
                }
            }
            NextCh();
        }

        res.Append(curstr);

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
            return "";
        }

        System.Console.WriteLine("Group of letters and digits is recognized");
        return res.ToString();
    }
}


public class DoubleLex : Lexer // Доп задание 2
{
    protected System.Text.StringBuilder intString;

    public DoubleLex(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsDigit(currentCh))
            NextCh();
        else
            Error();

        while (char.IsDigit(currentCh))
            NextCh();

        if (currentCh == '.')
            NextCh();

        while (char.IsDigit(currentCh))
            NextCh();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Double is recognized");

    }

    public override double SaveDouble()
    {
        NextCh();
        
        System.Text.StringBuilder res = new System.Text.StringBuilder();
        if (char.IsDigit(currentCh))
        {
            res.Append(currentCh);
            NextCh();
        }
        else
            Error();

        while (char.IsDigit(currentCh))
        {
            res.Append(currentCh);
            NextCh();
        }
           

        if (currentCh == '.')
        {
            res.Append(',');
            NextCh();
        }

        while (char.IsDigit(currentCh))
        {
            res.Append(currentCh);
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
            return 0;
        }

        System.Console.WriteLine("Double is recognized");

        return Convert.ToDouble(res.ToString());
    }
}


public class StringLex : Lexer // Доп задание 4
{
    protected System.Text.StringBuilder intString;

    public StringLex(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (currentCh == '\'')
            NextCh();
        else
            Error();

        while (char.IsLetter(currentCh))
            NextCh();
        

        if (currentCh == '\'')
            NextCh();
        else
            Error();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("String is recognized");

    }
    
}

public class Comment : Lexer // Доп задание 5
{
    protected System.Text.StringBuilder intString;

    public Comment(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (currentCh == '/')
            NextCh();
        else
            Error();

        if (currentCh == '*')
            NextCh();
        else
            Error();

        while (char.IsLetterOrDigit(currentCh))
            NextCh();


        if (currentCh == '*')
            NextCh();
        else
            Error();

        if (currentCh == '/')
            NextCh();
        else
            Error();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Comment is recognized");

    }

}

public class Program
{
    public static void Main()
    {
        string input = "-154216";
        Lexer L = new IntLexer(input);
        try
        {
            int res = L.Save();
            System.Console.WriteLine(res);
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        input = "a1231dfs3";
        L = new Identificator(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        input = "-013";
        L = new IntStartsNot0(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        input = "a4f4h2";
        L = new LetDig(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        input = "sfdsf,werw,hkkh;u";
        L = new StrWithMarks(input);
        try
        {
            L.SaveList();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        input = "wer   wrw yyyrtyr      er";
        L = new StrWithSpaces(input);
        try
        {
            L.SaveList();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        input = "a21ef4r55yu";
        L = new GroupLetDig(input);
        try
        {
            string s = L.SaveStr();
            System.Console.WriteLine(s);
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        input = "242.2342342";
        L = new DoubleLex(input);
        try
        {
            double d = L.SaveDouble();
            System.Console.WriteLine(d);
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "'строка'";
        L = new StringLex(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "/*выаы*/";
        L = new Comment(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.Read();

    }
}
