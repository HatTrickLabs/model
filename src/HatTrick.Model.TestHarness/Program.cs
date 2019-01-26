using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HatTrick.Model.MsSql;
using HatTrick.Reflection;

namespace HatTrick.Model.TestHarness
{
    class Program
    {
        /*********************************************************************************/
        /* To run this program as is, you must create an accessible sql server database 
        /* named MsSqlTest and run the \DbScripts\MsSqlTest.sql script located in this
        /* project to create the sample schema.  Or just change the connection string to point
        /* to any accessible Ms Sql Server Db and modify the overrides to work with your schema. 
        /*********************************************************************************/
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            //init builder
            MsSqlModelBuilder builder = new MsSqlModelBuilder("server=localhost;initial catalog=MsSqlTest;integrated security=true");

            //provide on error action callback
            bool error = false;
            builder.OnError += (ex) =>
            {
                error = true;
                Console.WriteLine($"Error!! {ex.Message}");
            };

            //build model
            MsSqlModel sqlModel = builder.Build();

            Resolve(sqlModel);

            if (!error)
            {
                //example remove an element
                sqlModel.Schemas["dbo"].Tables.Remove("TypeCode");

                //example override and element
                sqlModel.Schemas["dbo"].Tables["Address"].Columns["Line2"].Apply((c) =>
                {
                    c.Name = "AddressSet";
                    c.MaxLength = 50;
                    c.SqlType = SqlDbType.VarChar;
                    c.SqlTypeName = "varchar";
                    c.IsNullable = false;
                });
            }

            sw.Stop();
            Console.WriteLine($"processed in {sw.ElapsedMilliseconds} milliseconds.  Press [Enter] to exit.");
            Console.ReadLine();
        }

        static void Resolve(MsSqlModel model)
        {
            string key = @"dbo.Address.Line1\.Line2.Meta";

            TokenEmitter te = new TokenEmitter(key);
            te.Next = (tkn) =>
            {
                Console.WriteLine(tkn);
            };
            te.Parse();

            return;

            string[] keys = key.Split('.');

            MsSqlSchema s = model.Schemas[keys[0]];

            MsSqlTable t;
            MsSqlView v;
            MsSqlProcedure p;
            MsSqlRelationship r;
            for (int i = 0; i < keys.Length; i++)
            {
                t = s.Tables[keys[i]];
                if (t != null)
                {
                }

                v = s.Views[keys[i]];
                if (v != null)
                {
                }

                p = s.Procedures[keys[i]];
                if (p != null)
                {
                }

                r = s.Relationships[keys[i]];
                if (r != null)
                {
                }

            }

            //dbo.Address.Line2  => Name = AddressSet; MaxLen = 40
            //dbo.Address.AddressType => Type = AddressType;
            //dbo.TypeCode => Ignore = true

            //-Schemas
            //  - Tables
            //    - Columns
            //    - Indexes
            //  - Views
            //    - Columns
            //  - Procedures
            //    - Parameters
            //  - Relationships

        }
    }

    public class TokenEmitter
    {
        #region internals
        private char _delim = '.';
        private char _escape = '\\';

        private int _index;
        private int _length;

        private string _expression;

        private char[] _token;
        #endregion

        #region interface
        public Action<string> Next { get; set; }
        #endregion

        #region constructor
        public TokenEmitter(string expression)
        {
            _expression = expression;
            _length = expression.Length;
            _index = 0;
            _token = new char[128];
        }
        #endregion

        #region parse
        public void Parse()
        {
            string token;
            while ((token = this.Walk()) != null)
            {
                this.Next(token);
            }
        }
        #endregion

        #region walk
        private string Walk()
        {
            string token = null;
            char c;
            int len = 0;
            while (_index < _length && token == null)
            {
                c = _expression[_index];
                if (c == _escape && _expression[_index + 1] == _delim)
                {
                    _token[len++] = _expression[++_index];
                }
                else if (c == _delim)
                {
                    token = new string(_token, 0, len);
                }
                else
                {
                    _token[len++] = c;
                    if (_index == (_length - 1))
                    {
                        token = new string(_token, 0, len);
                    }
                }
                _index += 1;
            }

            return token;
        }
        #endregion
    }
}
