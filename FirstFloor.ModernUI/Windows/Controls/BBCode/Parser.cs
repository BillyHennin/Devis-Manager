// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from FirstFloor.ModernUI INC. team.
//  
// Copyrights (c) 2014 FirstFloor.ModernUI INC. All rights reserved.

using System;

namespace FirstFloor.ModernUI.Windows.Controls.BBCode
{
    internal abstract class Parser<TResult>
    {
        private readonly TokenBuffer buffer;

        protected Parser(Lexer lexer)
        {
            if(lexer == null)
            {
                throw new ArgumentNullException("lexer");
            }
            buffer = new TokenBuffer(lexer);
        }

        protected Token LA(int count)
        {
            return buffer.LA(count);
        }

        protected void Consume()
        {
            buffer.Consume();
        }

        protected bool IsInRange(params int[] tokenTypes)
        {
            if(tokenTypes == null)
            {
                return false;
            }

            Token token = LA(1);
            foreach(int t in tokenTypes)
            {
                if(token.TokenType == t)
                {
                    return true;
                }
            }

            return false;
        }

        protected void Match(int tokenType)
        {
            if(LA(1).TokenType == tokenType)
            {
                Consume();
            }
            else
            {
                throw new ParseException("Token mismatch");
            }
        }

        protected void MatchNot(int tokenType)
        {
            if(LA(1).TokenType != tokenType)
            {
                Consume();
            }
            else
            {
                throw new ParseException("Token mismatch");
            }
        }

        protected void MatchRange(int[] tokenTypes, int minOccurs, int maxOccurs)
        {
            int i = 0;
            while(IsInRange(tokenTypes))
            {
                Consume();
                i++;
            }
            if(i < minOccurs || i > maxOccurs)
            {
                throw new ParseException("Invalid number of tokens");
            }
        }

        public abstract TResult Parse();
    }
}