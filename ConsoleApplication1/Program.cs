using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("RuntimeAssembly")]
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i <= 100; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    Console.WriteLine("FizzBuzz");
                }

                else if (i%5==0)
                {
                    Console.WriteLine("Buzz");
                }
                else if(i%3==0)
                {
                    Console.WriteLine("Fizz");
                }
                else
                {
                    Console.WriteLine(i);
                }
            }

            //var res1=GenerateBC("http://google.ca/files/or-a-from-research-to-diplomatic/test.php", " . ");
            //var len1 = res1.Length;
            //var res2 = "<span class=\"active\">HOME</span>";
            //var len2 = res2.Length;

            //for (int i = 0; i < res1.Length; i++)
            //{
            //    char ch1 = res1[i];
            //    char ch2= res2[i];
            //    if (res1[i] != res2[i])
            //    {
            //        throw new Exception("Difference at character: " + i + 1);
            //    }
            //}
            //    var isEqual= String.Equals(res1, res2, StringComparison.InvariantCultureIgnoreCase);

            //Solution(50000);

            var properties = new Dictionary<string, Type> { { "SomeInt", typeof(int) }, { "SomeString", typeof(string) }, { "SomeObject", typeof(object) } };
            //Kata.DefineClass("SimpleClass", properties, ref myType);
            //myInstance = CreateInstance(myType);
            //myInstance.AString = "Hi";


            SampleClassParent obj = new SampleClassParent();
            var myType = CompileResultType("SomeClass",properties, obj.GetType());
            var myObject = Activator.CreateInstance(myType);


            ((SampleClassParent)myObject).SetValue<string>("SomeString", "Hello World!");
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                
            }
        }

        public static long NextSmaller(long n)
        {
            bool isNextSmallNumPoss = false;
            long tempNum = n;
            string str = n.ToString();
            var charList = str.GroupBy(x => x);
            while (tempNum > 9)
            {
                --tempNum;
                var tempList = tempNum.ToString().
                 GroupBy(k => k);
                var charArray = charList.Select(x => (tempList.Where(k => k.Key == x.Key && k.Count() == x.Count())).Count() != 0)
                 .ToList();

                if (charArray.All(x => x == true))
                {
                    isNextSmallNumPoss = true;
                    break;
                }
            }

            if (!isNextSmallNumPoss)
            {
                return -1;
            }
            return tempNum;
        }

        public static string GenerateBC(string url, string separator)
        {
            url = url.Replace("https://", "");
            url = url.Replace("http://", "");
            string[] paths = url.Split('/');

            paths = paths.Where(x => !x.Equals(string.Empty)).ToArray();

            var bcAsHTML=paths.Select((x, i) => (i == 0 && paths.Length>1 ? "<a href=\"/\">HOME</a>" :
            (i == paths.Length - 1  ? (x.ToLower().Contains("index") ? "" : (paths.Length==1?"": separator) + "<span class=\"active\">" + CheckIfMoreThan30(x,i) + "</span>") : separator+ string.Format("<a href=\"/{0}/\">{1}</a>",x.ToLower(),CheckIfMoreThan30(x,i))))).ToArray();
            bcAsHTML=bcAsHTML.Where(x => (!x.Equals(string.Empty))&& Regex.Match(x, @">([\w]+ {0,1}[\w]{0,})+<").Success).ToArray();
            if (bcAsHTML.Length!=paths.Length)
            {
                Match match= Regex.Match(bcAsHTML[bcAsHTML.Length - 1], @">([\w]+ {0,1}[\w]{0,})+<");
                bcAsHTML[bcAsHTML.Length - 1] = (bcAsHTML.Length==1?"":separator) + string.Format("<span class=\"active\"{0}/span>",match.Value);
            }
            
            var res = string.Concat(bcAsHTML);
            return res; 
        }

        public static string CheckIfMoreThan30(string str, int index)
        {
            str=index>0?(str.Split('.').Length>1? str.Split('.')[0]:str):"HOME";
            if (!(str.Length > 30))
                return str.ToUpper().Replace("-"," ").Split(new char[]{'#','?'})[0];
            string[] wordsToIgnore = new string[] { "the", "of", "in", "from", "by", "with", "and", "or", "for", "to", "at", "a" };
            string[] wordsArray = str.Split('-');
            wordsArray = wordsArray.Where(x => !wordsToIgnore.Contains(x)).ToArray();
            var res=string.Concat(wordsArray.Select(x => char.ToUpper(x[0])));
            return res;
        }


        public static string Solution(int n)
        {
            string NumString = "";
            int remainder = n;
            int thousand = remainder / 1000;
            remainder = (remainder % 1000) >= 0 ? remainder % 1000 : remainder;
            while (thousand > 0)
            {
                NumString += "M";
                thousand--;
            }

            int fiveHundred = remainder / 500;
            remainder = (remainder % 500) >= 0 ? remainder % 500 : remainder;
            while (fiveHundred > 0)
            {
                NumString += "D";
                fiveHundred--;
            }


            int hundreds = remainder / 100;
            remainder = (remainder % 100) >= 0 ? remainder % 100 : remainder;
            while (hundreds > 0)
            {
                NumString += "C";
                hundreds--;
            }

            int fifties = remainder / 50;
            remainder = (remainder % 50) >= 0 ? remainder % 50 : remainder;
            while (fifties > 0)
            {
                NumString += "L";
                fifties--;
            }

            int tens = remainder / 10;
            remainder = (remainder % 10) >= 0 ? remainder % 10 : remainder;
            while (tens > 0)
            {
                NumString += "X";
                tens--;
            }

            int fives = remainder / 5;
            remainder = (remainder % 5) >= 0 ? remainder % 5 : remainder;
            while (fives > 0)
            {
                NumString += "V";
                fives--;
            }

            int ones = remainder / 1;
            remainder = (remainder % 1) >= 0 ? remainder % 1 : remainder;
            while (ones > 0)
            {
                NumString += "I";
                ones--;
            }

            return NumString;
        }

        public static Type CompileResultType(string className, Dictionary<string, Type> properties,Type type)
        {
            TypeBuilder tb = GetTypeBuilder(className,type);
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            // NOTE: assuming your list contains Field objects with fields FieldName(string) and FieldType(Type)
            foreach (var field in properties)
                CreateProperty(tb, field.Key, field.Value);

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder(string className, Type type)
        {
            var typeSignature = "RuntimeAssembly";
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(className,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    type);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField(propertyName, propertyType, FieldAttributes.Public);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }

    }
}
