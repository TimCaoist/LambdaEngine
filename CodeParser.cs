using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine
{
    public static class CodeParser
    {
        public static CodePiece Parser(string code)
        {
            var codePiece = new CodePiece();
            var index = 0;
            var len = code.Length;
            ICollection<char> text = new List<char>();
            Action<char> collectionStr = (c) =>
            {
                var str = new string(text.ToArray());
                if (!string.IsNullOrWhiteSpace(str))
                {
                    codePiece.Tokens.Add(new Token(str.Trim(), index));
                }

                text.Clear();
                text.Add(c);
            };

            Func<char, char[], bool> continueStr = (c, cs) =>
            {
                var nextC = code[index + 1];
                if (cs.Contains(nextC))
                {
                    text.Add(c);
                    text.Add(nextC);
                    index += 2;
                    return true;
                }

                return false;
            };

            bool isSplit = false;
            while (index < len)
            {
                var c = code[index];
                switch (c)
                {
                    case '=':
                        {
                            isSplit = true;
                            var nextC = code[index + 1];
                            var r = continueStr(c, new char[] { '=', '>' });
                            if (r == true)
                            {
                                continue;
                            }

                            collectionStr(c);
                            break;
                        }
                    case '+':
                        {
                            isSplit = true;
                            var nextC = code[index + 1];
                            var r = continueStr(c, new char[] { '+' });
                            if (r == true)
                            {
                                continue;
                            }

                            collectionStr(c);
                            break;
                        }
                    case '-':
                        {
                            isSplit = true;
                            var nextC = code[index + 1];
                            var r = continueStr(c, new char[] { '-' });
                            if (r == true)
                            {
                                continue;
                            }

                            collectionStr(c);
                            break;
                        }
                    case '>':
                    case '<':
                        {
                            isSplit = true;
                            var nextC = code[index + 1];
                            var r = continueStr(c, new char[] { '=' });
                            if (r == true)
                            {
                                continue;
                            }

                            collectionStr(c);
                            break;
                        }
                    case ' ':
                    case ',':
                    case ';':
                    case '*':
                    case '/':
                    case '(':
                    case ')':
                    case '}':
                    case '{':
                    case ':':
                        {
                            isSplit = true;
                            collectionStr(c);
                            break;
                        }

                    default:
                        {
                            if (isSplit == true)
                            {
                                isSplit = false;
                                collectionStr(c);
                                break;
                            }

                            text.Add(c);
                            break;
                        }
                }

                index++;
            }

            if (!text.Any())
            {
                throw new ArgumentException("语法错误!");
            }

            var str1 = new string(text.ToArray());
            if (!string.IsNullOrWhiteSpace(str1))
            {
                codePiece.Tokens.Add(new Token(str1.Trim(), code.Length - 1));
            }

            codePiece.Variables = VariableParser.Parser(codePiece);
            return codePiece;
        }
    }
}
