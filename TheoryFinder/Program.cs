using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Build;
using Microsoft.Build.Locator;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace TheoryFinder
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0 || !File.Exists(args[0]))
            {
                Console.WriteLine("Invalid Input");
                return;
            }

            //if (!Path.GetExtension(args[0]).Equals(".sln"))
            //{
            //    Debug.Assert(Path.GetExtension(args[0]).Equals(".slnf"), "input args[0] should be a solution file!");
            //}
            Debug.Assert(Path.GetExtension(args[0]).Equals(".sln"), "input args[0] should be a solution file!");

            string sln = Path.GetFullPath(args[0]);
            // Attempt to set the version of MSBuild.
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            var instance = visualStudioInstances.Length == 1
                // If there is only one instance of MSBuild on this machine, set that as the one to use.
                ? visualStudioInstances[0]
                // Handle selecting the version of MSBuild you want to use.
                : SelectVisualStudioInstance(visualStudioInstances);

            // NOTE: Be sure to register an instance with the MSBuildLocator 
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
            MSBuildLocator.RegisterInstance(instance);

            Console.WriteLine($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");


            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Console.WriteLine("####");
            Solution solution = workspace.OpenSolutionAsync(sln).Result; //Error is here

            Console.WriteLine("setting up");

            #region Setup


            //List of possible PUT attributes 
            List<String> putAttributes = new List<string>
            {
                "Theory",
                "DataSource",
                "XxxData",
                "TestCaseSource"
            };

            //List of possible conventional unit test attributes
            List<String> unitTestAttributes = new List<string>
            {
                "Test",
                "Fact",
                "TestCase"
            };



            List<String> assertStatements = new List<string>
            {
                "Assert",
                "AssertUpdateNotCalled",
                "AssertUpdateConsentCalled",
                "AssertUpdateConsentNotCalled",
                "AssertStandardEnvironments",
                "AssertValidToken",
                "AssertEscaped",
                "AssertJobIsCompleteDueToTaskFailure",
                "AssertItem",
                "AssertTextRange",
                "AssertMenuItemTextAndLinkedSortType",
                "AssertOnlyCheckedItemIs",
                "AssertListCount",
                "AssertScope",
                "AssertHasLog(",
                "AssertHasLogRpcConnectionError",
                "AssertExtension.ValidationResultIsError",
                "AssertExtension.ValidationResultIsSuccess",
                "AssertExists",
                "AssertDoesNotExist",
                "AssertMessagesWithCursorForRange",
                "AssertNoTargetSite",
                "AssertAll",
                "AssertNone",
                "AssertValueConverted",
                "AssertDecimalTryParse",
                "AssertNewDateTimeParseEqual",
                "AssertNewDateTimeOffsetParseEqual",
                "AssertIsProperty",
                "AssertIsIndexer",
                "AssertSingle",
                "AssertChildrenSelected",
                "AssertArePixelEqual",
                "AssertThrows(",
                "AssertContains(",
                "AssertConstantField<",
                "AssertParseFailure",
                "AssertValue(",
                "AssertTrue",
                "AssertFalse",
                "AssertEqualLink",
                "AssertEqualListViewSubItem",
                "AssertToObjectEqual",
                "AssertExtensions.Throws",
                "AssertAttributes",
                "AssertTopologicallySorted",
                "AssertEx.Equal(",
                "AssertEx.Empty",
                "AssertEx.SetEqual",
                "AssertEx.AssertEqualToleratingWhitespaceDifferences",
                "AssertEx.NotNull",
                "AssertEx.Fail",
                "AssertChangedTextLinesHelper",
                "AssertSpecificDiagnostics",
                "AssertNoAttributes",
                "AssertReferencedIsByRefLike",
                "AssertDeclaresType",
                "AssertNoIsByRefLikeAttributeExists",
                "AssertGeneratedEmbeddedAttribute",
                "AssertNotReferencedIsByRefLikeAttribute",
                "AssertReferencedIsUnmanagedAttribute",
                "AssertNoIsUnmanagedAttributeExists",
                "AssertNativeIntegerAttributes",
                "AssertNativeIntegerAttribute(",
                "AssertNoNativeIntegerAttributes",
                "AssertNoNullableAttributeAssertNullableAttributes",
                "AssertNullableAttribute",
                "AssertNoIsReadOnlyAttributeExists",
                "AssertProperty",
                "AssertEx.All",
                "AssertEx.None",
                "AssertTupleTypeEquality",
                "AssertTestDisplayString",
                "AssertTupleNonElementField",
                "AssertNonvirtualTupleElementField",
                "AssertVirtualTupleElementField",
                "AssertTupleTypeMembersEquality",
                "AssertNotInstrumented",
                "AssertInstrumented",
                "AssertOuterIsCorrespondingLoopOfInner",
                "AssertOuterIsCorrespondingSwitchOfInner",
                "AssertEnabledForInheritence",
                "AssertGetSpeculativeTypeInfo",
                "AssertTryGetSpeculativeSemanticModel",
                "AssertHashCodesMatch",
                "AssertDiagnosticOptions_NullableWarningsGiven_OnlyWhenEnabledInProject",
                "AssertDiagnosticOptions_NullableWarningsNeverGiven",
                "AssertDiagnosticOptions_NullableWarningsGiven_UnlessDisabledByDiagnosticOptions",
                "AssertDiagnosticOptions_NullableW_WarningsGiven_OnlyWhenEnabledInProject",
                "AssertDiagnosticOptions_NullableW_WarningsGiven_UnlessDisabledByDiagnosticOptions",
                "AssertEqual(",
                "AssertContainedInDeclaratorArguments",
                "AssertEmpty",
                "AssertCompilationCorlib",
                "AssertCannotConstruct",
                "AssertRuntimeFeatureTrue",
                "AssertNoMethodImplementation",
                "AssertNoPropertyImplementation",
                "AssertNoEventImplementation",
                "AssertEqualityAndHashCode",
                "AssertSame",
                "AssertNoParameterHasModOpts",
                "AssertPos",
                "AssertCanUnify",
                "AssertCannotUnify",
                "AssertMappedSpanEqual",
                "AssertEx.AreEqual",
                "AssertTokens",
                "AssertGoodDecimalLiteral",
                "AssertBadDecimalLiteral",
                "AssertEqualRoundtrip",
                "AssertCompleteSubmission",
                "AssertIncompleteSubmission",
                "AssertTrimmedEqual",
                "AssertFormatCSharp",
                "AssertFormatVB",
                "AssertNamesEqual",
                "AssertMemberNamesEqual",
                "AssertMembers",
                "AssertEx.EqualOrDiff",
                "AssertRelativeOrder",
                "AssertSymbolKeysEqual",
                "AssertExtent",
                "AssertDiagnostics",
                "AssertContainsType",
                "AssertIsIntPtrPointer",
                "AssertDumpsEqual",
                "AssertJsonEquals",
                "AssertLocationsEqual",
                "AssertServerCapabilities",
                "AssertEx.NotEqual(",
                "AssertNoNullableAttribute(",
                "AssertNoNullableAttributes(",
                "AssertNotReferencedIsReadOnlyAttribute(",
                "AssertEntryPointParameter(",
                "AssertRuntimeFeatureFalse",
                "AssertEx.Any(",
                "All",
                "AreEqual",
                "AreNotEqual",
                "AreSame",
                "AreNotSame",
                "Collection",
                "Contains",
                "DoesNotContain",
                "DoesNotThrow",
                "Empty(",
                "EndsWith",
                "Equal",
                "Equals",
                "Fail",
                "False",
                "Greater(",
                "GreaterOrEqual(",
                "Ignore",
                "InRange",
                "Inconclusive",
                "IsAssignableFrom",
                "IsEmpty",
                "IsFalse",
                "IsInstanceOf",
                "IsNotEmpty",
                "IsNotNull",
                "IsNotType",
                "IsNull",
                "IsType",
                "IsTrue",
                "Less(",
                "Less (",
                "LessOrEqual",
                "Matches",
                "NotEmpty",
                "NotEqual",
                "NotInRange",
                "NotNull",
                "NotSame",
                "NotStrictEqual",
                "Null",
                "Pass",
                "ProperSubset",
                "ProperSuperset",
                "PropertyChanged",
                "PropertyChangedAsync",
                "RaisedEvent",
                "Raises",
                "RaisesAny",
                "RaisesAnyAsync",
                "RaisesAsync",
                "ReferenceEquals",
                "Same",
                "Single",
                "StartsWith",
                "StrictEqual",
                "Subset",
                "Superset",
                "Throws<",
                "Throws(",
                "Throws (",
                "ThrowsAny<",
                "ThrowsAnyAsync",
                "ThrowsAsync",
                "That",
                "True"

            };
            #endregion
            //key: document name, value: put count, unit count
            Dictionary<string, Tuple<int, int>> mapDocToRatios = new Dictionary<string, Tuple<int, int>>();


            // key: project name value: ratios 
            Dictionary<string, Tuple<int, int>> mapProjToRatios = new Dictionary<string, Tuple<int, int>>();

            //TODO: Anna knows 0
            // key: doc name value: test method declarations
            Dictionary<string, IEnumerable<MethodDeclarationSyntax>> mapDocToPUTNames = new Dictionary<string, IEnumerable<MethodDeclarationSyntax>>();
            Dictionary<string, IEnumerable<MethodDeclarationSyntax>> mapDocToUnitNames = new Dictionary<string, IEnumerable<MethodDeclarationSyntax>>();

            int TheoryCount = 0;
            int TotalCount = 0;

            int putLineCount = 0;
            int unitTestLineCount;


            bool visitedProject = false;
            foreach (Project project in solution.Projects)
            {

                if (Path.GetExtension(project.Name).Equals(".vcproj"))
                    continue;
                visitedProject = true;
                int theoryProjCount = 0;
                int unitProjCount = 0;

                foreach (Document document in project.Documents)
                {

                    //TODOs:
                    //  Main: Angello wants Anna and Charles to impress professors
                    // Prioritize: a) Send me ratios of PUTs/ test methods, of UnitTest/test method for a solution  --> tonight! (google sheets)
                    // Get it done Real quick: b) Do this per test project
                    // Get it done Real quick: c) Do this per test class.
                    // Prioritize: Anna knows 0
                    // Not Prioritize Related Work
                    // filte document/project by test (check "Test", "test", "Tests" in class name) and value is tuple of counts see line 78
                    //if test in name:
                    //    continue

                    //commented out b/c certain documents without the keywords contain PUTs
                    //if (!(document.Name.Contains("test") || document.Name.Contains("Test") || document.Name.Contains("Tests") || document.Name.Contains("tests")))
                    //{
                    //Charles knows 0
                    //Console.WriteLine(document.Name);
                    //continue;
                    //}

                    string filepath = document.FilePath;
                    string text = File.ReadAllText(filepath);

                    //int numTestMethods = getUnitTestMethods(text, "Test");
                    //int numFactMethods = getUnitTestMethods(text, "Fact");
                    //int numTheoryMethods = getPUTMethods(text, "Theory");

                    if (!mapDocToPUTNames.ContainsKey(filepath))
                    {
                        IEnumerable<MethodDeclarationSyntax> toAdd = getTestingMethods(text, putAttributes);
                        if (toAdd.Count() > 0)
                        {
                            mapDocToPUTNames.Add(filepath, toAdd);
                            //Console.WriteLine(filepath);

                        }

                    }
                    if (!mapDocToUnitNames.ContainsKey(filepath))
                    {
                        IEnumerable<MethodDeclarationSyntax> toAdd = getTestingMethods(text, unitTestAttributes);
                        if (toAdd.Count() > 0)
                        {
                            mapDocToUnitNames.Add(filepath, toAdd);

                        }

                    }



                    int numUnitMethods = getAttributeCount(text, unitTestAttributes);
                    int numPUTMethods = getAttributeCount(text, putAttributes);

                    //int numUnitMethods = numTestMethods + numFactMethods;
                    TheoryCount += numPUTMethods;
                    TotalCount += (numUnitMethods + numPUTMethods);
                    theoryProjCount += numPUTMethods;
                    unitProjCount += (numUnitMethods);
                    if (numUnitMethods != 0 || numPUTMethods != 0)
                    {
                        //Anna an Charles will did not run this but Angello think will work.
                        //mapDocToRatios.Add(document.Name, new Tuple<int, int>(numPUTMethods, numUnitMethods));
                        //Console.WriteLine(numPUTMethods + " PUTs and " + numUnitMethods + " Unit Tests in " + filepath);


                    }


                }
                //not for test projects
                mapProjToRatios.Add(project.Name, new Tuple<int, int>(theoryProjCount, unitProjCount));
                //Console.WriteLine(theoryProjCount + "PUTs " + unitProjCount + "Unit Tests for " + project.Name);
                //Console.WriteLine(project.FilePath);
            }

            //Anna knows 6
            if (!visitedProject)
                Console.WriteLine("Unsure if unit test are in project");
            else
                Console.WriteLine("The ratio of PUT methods to unit test methods is " + TheoryCount + ":" + (TotalCount - TheoryCount));
            //printFirstLine(mapDocToPUTNames);
            //printFirstLine(mapDocToUnitNames);
            Console.WriteLine(getTotalTestLines(mapDocToPUTNames) + " PUT Lines");
            Console.WriteLine(getTotalTestLines(mapDocToUnitNames) + " Unit Test Lines");

            Dictionary<string, int> putAssertCount = getAssertCount(assertStatements, mapDocToPUTNames);
            foreach (KeyValuePair<string, int> element in putAssertCount)
            {
                if (element.Value > 0)
                {
                    Console.WriteLine(element.Value + " instances of Assert." + element.Key + " in PUT Tests");
                }
            }

            Dictionary<string, int> unitAssertCount = getAssertCount(assertStatements, mapDocToUnitNames);
            foreach (KeyValuePair<string, int> element in unitAssertCount)
            {
                if (element.Value > 0)
                {
                    Console.WriteLine(element.Value + " instances of Assert." + element.Key + " in Unit Tests");
                }
            }
            /*
            Dictionary<string, List<List<ParameterSyntax>>> assertParameters = getAssertParameters(assertStatements, mapDocToPUTNames);
            foreach(KeyValuePair<string, List<List<ParameterSyntax>>> element in assertParameters)
            {
                if (element.Value.Count == 0)
                {
                    continue;
                }
                foreach(List<ParameterSyntax> parameterList in element.Value)
                {
                    Console.Write(element.Key + " ");
                    foreach(ParameterSyntax parameter in parameterList)
                    {
                        Console.Write(parameter.Type + " " + parameter.ToString() + ", ");
                    }
                    Console.WriteLine();
                }
            }*/
            /*Dictionary<string, List<List<string>>> assertParameters = getAssertParametersAsString(assertStatements, mapDocToPUTNames);
            foreach (KeyValuePair<string, List<List<string>>> element in assertParameters)
            {
                if (element.Value.Count == 0)
                {
                    continue;
                }
                foreach (List<string> parameterList in element.Value)
                {
                    Console.Write(element.Key);
                    foreach (string parameter in parameterList)
                    {
                        Console.Write(" " + parameter + ", ");
                    }
                    Console.WriteLine();
                }
            }*/




        }


        //Dictionary can contain projects w/out PUTs/Unit Tests
        //Charles Knows 4
        //Add code the average lines of code per PUT/Unit Test in a dictionary
        public static void printFirstLine(Dictionary<string, IEnumerable<MethodDeclarationSyntax>> dictionary)
        {
            //print first line of each method
            //Charles Knows 1
            //Google how to iterate through statements in methodDeclarationSyntax
            //Maybe use blocksyntax body()?
            int i = 0;
            foreach (KeyValuePair<string, IEnumerable<MethodDeclarationSyntax>> element in dictionary)
            {
                foreach (MethodDeclarationSyntax method in element.Value)
                {
                    Console.Write(i);
                    List<StatementSyntax> expressionNodes = method.DescendantNodes().OfType<StatementSyntax>().ToList();
                    int j = 0;
                    foreach (StatementSyntax statement in expressionNodes)
                    {
                        //Console.Write(" " + j);
                        //Console.WriteLine(statement.ToString());
                        Console.WriteLine();
                    }
                    j++;
                }
                i++;
            }
        }

        public static double getAverageTestSize(Dictionary<string, IEnumerable<MethodDeclarationSyntax>> dictionary)
        {

            int totalLines = 0;
            int numMethods = 0;
            foreach (KeyValuePair<string, IEnumerable<MethodDeclarationSyntax>> element in dictionary)
            {
                foreach (MethodDeclarationSyntax method in element.Value)
                {
                    numMethods++;
                    List<StatementSyntax> expressionNodes = method.DescendantNodes().OfType<StatementSyntax>().ToList();

                    foreach (StatementSyntax statement in expressionNodes)
                    {
                        totalLines++;
                        //Console.Write(" " + j);
                        //Console.WriteLine(statement.ToString());

                    }
                }
            }
            return (double)totalLines / (double)numMethods;
        }

        public static int getNumTestMethods(Dictionary<string, IEnumerable<MethodDeclarationSyntax>> dictionary)
        {


            int numMethods = 0;
            foreach (KeyValuePair<string, IEnumerable<MethodDeclarationSyntax>> element in dictionary)
            {
                foreach (MethodDeclarationSyntax method in element.Value)
                {
                    numMethods++;

                }
            }
            return numMethods;
        }

        public static int getTotalTestLines(Dictionary<string, IEnumerable<MethodDeclarationSyntax>> dictionary)
        {

            int totalLines = 0;

            foreach (KeyValuePair<string, IEnumerable<MethodDeclarationSyntax>> element in dictionary)
            {
                foreach (MethodDeclarationSyntax method in element.Value)
                {

                    List<StatementSyntax> expressionNodes = method.DescendantNodes().OfType<StatementSyntax>().ToList();

                    foreach (StatementSyntax statement in expressionNodes)
                    {
                        totalLines++;
                        //Console.Write(" " + j);
                        //Console.WriteLine(statement.ToString());

                    }
                }
            }
            return totalLines;
        }

        public static Dictionary<string, int> getAssertCount(List<string> asserts, Dictionary<string, IEnumerable<MethodDeclarationSyntax>> dictionary)
        {
            Dictionary<string, int> assertCount = new Dictionary<string, int>();
            foreach (string assertion in asserts)
            {

                int attributeCount = 0;
                string assertStatement;
                if (assertion.StartsWith("Assert"))
                {
                    assertStatement = assertion;
                }
                else
                {
                    assertStatement = "Assert." + assertion;
                }
                foreach (KeyValuePair<string, IEnumerable<MethodDeclarationSyntax>> element in dictionary)
                {
                    foreach (MethodDeclarationSyntax method in element.Value)
                    {

                        List<StatementSyntax> expressionNodes = method.DescendantNodes().OfType<StatementSyntax>().ToList();

                        foreach (StatementSyntax statement in expressionNodes)
                        {
                            if (statement.ToString().StartsWith(assertStatement))
                            {
                                //if (assertStatement.Equals("Assert") && !statement.ToString().StartsWith("Assert."))
                                if (assertStatement.Equals("Assert"))
                                {
                                    //check if the assert isn't contained in the list
                                    bool newStatement = true;


                                    for (int i = 1; i < asserts.Count() && newStatement; i++)
                                    {
                                        string toCheck;
                                        if (asserts[i].StartsWith("Assert"))
                                        {
                                            toCheck = asserts[i];
                                        }
                                        else
                                        {
                                            toCheck = "Assert." + asserts[i];
                                        }
                                        if (statement.ToString().Contains(toCheck))
                                        {
                                            newStatement = false;
                                        }
                                    }
                                    if (newStatement)
                                    {
                                        Console.WriteLine(statement);
                                    }

                                }
                                attributeCount++;
                            }

                        }
                    }
                }
                assertCount.Add(assertion, attributeCount);
            }

            return assertCount;
        }

        public static Dictionary<string, List<List<ParameterSyntax>>> getAssertParameters(List<string> asserts, Dictionary<string, IEnumerable<MethodDeclarationSyntax>> dictionary)
        {
            Dictionary<string, List<List<ParameterSyntax>>> assertParameters = new Dictionary<string, List<List<ParameterSyntax>>>();
            int i = 0;
            foreach (string assertion in asserts)
            {
                string assertStatement;
                if (assertion.Equals("Assert"))
                {
                    continue;
                }
                else if (assertion.StartsWith("Assert"))
                {
                    assertStatement = assertion;
                }
                else
                {
                    assertStatement = "Assert." + assertion;
                }
                List<List<ParameterSyntax>> parameters = new List<List<ParameterSyntax>>();

                foreach (KeyValuePair<string, IEnumerable<MethodDeclarationSyntax>> element in dictionary)
                {
                    foreach (MethodDeclarationSyntax method in element.Value)
                    {

                        List<StatementSyntax> expressionNodes = method.DescendantNodes().OfType<StatementSyntax>().ToList();
                        foreach (StatementSyntax statement in expressionNodes)
                        {
                            //if (statement.ToString().StartsWith(assertStatement) && statement.ToString().IndexOf('(') > 0 && statement.ToString().LastIndexOf(')') != -1)
                            if (statement.ToString().StartsWith(assertStatement))
                            {
                                /*Console.WriteLine(statement.ToString());
                                Console.WriteLine("Length: " + statement.ToString().Length);
                                Console.WriteLine("( index " + statement.ToString().IndexOf('('));
                                Console.WriteLine(") index " + statement.ToString().LastIndexOf(')'));
                                Console.WriteLine(statement.ToString().Substring(1 + statement.ToString().IndexOf('('), statement.ToString().LastIndexOf(')') - statement.ToString().IndexOf('(') - 1));
                                */


                                List<ParameterSyntax> parameterList = statement.DescendantNodes().OfType<ParameterSyntax>().ToList();
                                parameters.Add(parameterList);
                            }


                        }
                    }
                }
                assertParameters.Add(assertStatement, parameters);
            }



            return assertParameters;
        }

        public static Dictionary<string, List<List<string>>> getAssertParametersAsString(List<string> asserts, Dictionary<string, IEnumerable<MethodDeclarationSyntax>> dictionary)
        {
            Dictionary<string, List<List<string>>> assertParameters = new Dictionary<string, List<List<string>>>();
            int i = 0;
            foreach (string assertion in asserts)
            {
                string assertStatement;
                if (assertion.Equals("Assert"))
                {
                    continue;
                }
                else if (assertion.StartsWith("Assert"))
                {
                    assertStatement = assertion;
                }
                else
                {
                    assertStatement = "Assert." + assertion;
                }
                List<List<string>> parameters = new List<List<string>>();

                foreach (KeyValuePair<string, IEnumerable<MethodDeclarationSyntax>> element in dictionary)
                {
                    foreach (MethodDeclarationSyntax method in element.Value)
                    {

                        List<StatementSyntax> expressionNodes = method.DescendantNodes().OfType<StatementSyntax>().ToList();
                        foreach (StatementSyntax statement in expressionNodes)
                        {
                            //if (statement.ToString().StartsWith(assertStatement) && statement.ToString().IndexOf('(') > 0 && statement.ToString().LastIndexOf(')') != -1)
                            if (statement.ToString().StartsWith(assertStatement))
                            {
                                /*Console.WriteLine(statement.ToString());
                                Console.WriteLine("Length: " + statement.ToString().Length);
                                Console.WriteLine("( index " + statement.ToString().IndexOf('('));
                                Console.WriteLine(") index " + statement.ToString().LastIndexOf(')'));
                                Console.WriteLine(statement.ToString().Substring(1 + statement.ToString().IndexOf('('), statement.ToString().LastIndexOf(')') - statement.ToString().IndexOf('(') - 1));
                                */

                                string parametersAsString = statement.ToString().Substring(statement.ToString().IndexOf('(') + 1, statement.ToString().LastIndexOf(')') - statement.ToString().IndexOf('(') - 1);
                                List<string> parameterList = parametersAsString.Split(',').Select(s => s.Trim()).ToList();
                                parameters.Add(parameterList);
                                /*
                                List<ParameterSyntax> parameterList = statement.DescendantNodes().OfType<ParameterSyntax>().ToList();
                                parameters.Add(parameterList);*/
                            }


                        }
                    }
                }
                assertParameters.Add(assertStatement, parameters);
            }



            return assertParameters;
        }


        /* args: Test class text, attribute name
         * returns: Method declarations of attribute name 
         */
        public static IEnumerable<MethodDeclarationSyntax> getTestingMethods(string classTextContent, string attributeName)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(classTextContent);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            IEnumerable<MethodDeclarationSyntax> MyMethod = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(p => p.DescendantNodes()
             .OfType<AttributeSyntax>()
             .Any(a => a.DescendantNodes()
                 .OfType<IdentifierNameSyntax>()
                 .Any(i => i.DescendantTokens()
                     .Any(dt => dt.ValueText == attributeName))));

            return MyMethod;
        }

        public static IEnumerable<MethodDeclarationSyntax> getTestingMethods(string classTextContent, List<string> attributes)
        {
            if (attributes == null)
            {
                return null;
            }
            IEnumerable<MethodDeclarationSyntax> toReturn = Enumerable.Empty<MethodDeclarationSyntax>();
            foreach (string attribute in attributes)
            {
                toReturn = toReturn.Concat(getTestingMethods(classTextContent, attribute));
            }

            return toReturn;
        }

        public static int getPUTMethods(string classTextContent, string attributeName)
        {
            return getTestingMethods(classTextContent, attributeName).Count();
        }

        public static int getUnitTestMethods(string classTextContent, string attributeName)
        {
            return getTestingMethods(classTextContent, attributeName).Count();
        }

        public static int getAttributeCount(string classTextContent, List<string> attributes)
        {
            int count = 0;
            foreach (string attribute in attributes)
            {
                count += getTestingMethods(classTextContent, attribute).Count();
            }
            return count;
        }


        private static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
            Console.WriteLine("Multiple installs of MSBuild detected please select one:");
            for (int i = 0; i < visualStudioInstances.Length; i++)
            {
                Console.WriteLine($"Instance {i + 1}");
                Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
                Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
                Console.WriteLine($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
            }

            while (true)
            {
                var userResponse = Console.ReadLine();
                if (int.TryParse(userResponse, out int instanceNumber) &&
                    instanceNumber > 0 &&
                    instanceNumber <= visualStudioInstances.Length)
                {
                    return visualStudioInstances[instanceNumber - 1];
                }
                Console.WriteLine("Input not accepted, try again.");
            }
        }
    }
}