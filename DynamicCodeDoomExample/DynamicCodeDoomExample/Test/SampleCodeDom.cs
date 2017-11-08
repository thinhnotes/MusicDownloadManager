using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

namespace DynamicCodeDoomExample
{
    /// <summary>
    ///     This code example creates a graph using a CodeCompileUnit and
    ///     generates source code for the graph using the CSharpCodeProvider.
    /// </summary>
    internal class Sample
    {
        /// <summary>
        ///     The only class in the compile unit. This class contains 2 fields,
        ///     3 properties, a constructor, an entry point, and 1 simple method.
        /// </summary>
        private readonly CodeTypeDeclaration _targetClass;

        /// <summary>
        ///     Define the compile unit to use for code generation.
        /// </summary>
        private readonly CodeCompileUnit _targetUnit;

        /// <summary>
        ///     Define the class.
        /// </summary>
        public Sample()
        {
            _targetUnit = new CodeCompileUnit();
            var samples = new CodeNamespace("CodeDOMSample");
            samples.Imports.Add(new CodeNamespaceImport("System"));
            _targetClass = new CodeTypeDeclaration("CodeDOMCreatedClass");
            _targetClass.IsClass = true;
            _targetClass.TypeAttributes =
                TypeAttributes.Public | TypeAttributes.Sealed;
            samples.Types.Add(_targetClass);
            _targetUnit.Namespaces.Add(samples);
        }

        /// <summary>
        ///     Adds two fields to the class.
        /// </summary>
        public void AddFields()
        {
            // Declare the widthValue field.
            var widthValueField = new CodeMemberField();
            widthValueField.Attributes = MemberAttributes.Private;
            widthValueField.Name = "widthValue";
            widthValueField.Type = new CodeTypeReference(typeof (double));
            widthValueField.Comments.Add(new CodeCommentStatement(
                "The width of the object."));
            _targetClass.Members.Add(widthValueField);

            // Declare the heightValue field
            var heightValueField = new CodeMemberField();
            heightValueField.Attributes = MemberAttributes.Private;
            heightValueField.Name = "heightValue";
            heightValueField.Type =
                new CodeTypeReference(typeof (double));
            heightValueField.Comments.Add(new CodeCommentStatement(
                "The height of the object."));
            _targetClass.Members.Add(heightValueField);
        }

        /// <summary>
        ///     Add three properties to the class.
        /// </summary>
        public void AddProperties(string propertyName, Type type)
        {
            var field = new CodeMemberField
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = propertyName,
                Type = new CodeTypeReference(type),
            };

            field.Name += "{ get; set; }";
            _targetClass.Members.Add(field);

        }

        /// <summary>
        ///     Adds a method to the class. This method multiplies values stored
        ///     in both fields.
        /// </summary>
        public void AddMethod()
        {
            // Declaring a ToString method
            var toStringMethod = new CodeMemberMethod();
            toStringMethod.Attributes =
                MemberAttributes.Public | MemberAttributes.Override;
            toStringMethod.Name = "ToString";
            toStringMethod.ReturnType =
                new CodeTypeReference(typeof (string));

            var widthReference =
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), "Width");
            var heightReference =
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), "Height");
            var areaReference =
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), "Area");

            // Declaring a return statement for method ToString.
            var returnStatement =
                new CodeMethodReturnStatement();

            // This statement returns a string representation of the width, 
            // height, and area. 
            var formattedOutput = "The object:" + Environment.NewLine +
                                  " width = {0}," + Environment.NewLine +
                                  " height = {1}," + Environment.NewLine +
                                  " area = {2}";
            returnStatement.Expression =
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression("System.String"), "Format",
                    new CodePrimitiveExpression(formattedOutput),
                    widthReference, heightReference, areaReference);
            toStringMethod.Statements.Add(returnStatement);
            _targetClass.Members.Add(toStringMethod);
        }

        /// <summary>
        ///     Add a constructor to the class.
        /// </summary>
        public void AddConstructor()
        {
            // Declare the constructor
            var constructor = new CodeConstructor();
            constructor.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;

            // Add parameters.
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof (double), "width"));
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof (double), "height"));

            // Add field initialization logic
            var widthReference =
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), "widthValue");
            constructor.Statements.Add(new CodeAssignStatement(widthReference,
                new CodeArgumentReferenceExpression("width")));
            var heightReference =
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), "heightValue");
            constructor.Statements.Add(new CodeAssignStatement(heightReference,
                new CodeArgumentReferenceExpression("height")));
            _targetClass.Members.Add(constructor);
        }

        /// <summary>
        ///     Add an entry point to the class.
        /// </summary>
        public void AddEntryPoint()
        {
            var start = new CodeEntryPointMethod();
            var objectCreate =
                new CodeObjectCreateExpression(
                    new CodeTypeReference("CodeDOMCreatedClass"),
                    new CodePrimitiveExpression(5.3),
                    new CodePrimitiveExpression(6.9));

            // Add the statement: 
            // "CodeDOMCreatedClass testClass =  
            //     new CodeDOMCreatedClass(5.3, 6.9);"
            start.Statements.Add(new CodeVariableDeclarationStatement(
                new CodeTypeReference("CodeDOMCreatedClass"), "testClass",
                objectCreate));

            // Creat the expression: 
            // "testClass.ToString()"
            var toStringInvoke =
                new CodeMethodInvokeExpression(
                    new CodeVariableReferenceExpression("testClass"), "ToString");

            // Add a System.Console.WriteLine statement with the previous  
            // expression as a parameter.
            start.Statements.Add(new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.Console"),
                "WriteLine", toStringInvoke));
            _targetClass.Members.Add(start);
        }

        /// <summary>
        ///     Generate CSharp source code from the compile unit.
        /// </summary>
        /// <param name="filename">Output file name</param>
        public void GenerateCSharpCode(string fileName)
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (var sourceWriter = new StreamWriter(fileName))
            {
                provider.GenerateCodeFromCompileUnit(
                    _targetUnit, sourceWriter, options);
            }
        }
    }
}