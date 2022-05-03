USE [CCS]
GO
INSERT [dbo].[Teacher] ([teacher_id]) VALUES (CAST(2876 AS Numeric(7, 0))) --userco
GO
INSERT [dbo].[Teacher] ([teacher_id]) VALUES (CAST(2203 AS Numeric(7, 0))) --userte
GO
INSERT [dbo].[Teacher] ([teacher_id]) VALUES (CAST(1427 AS Numeric(7, 0))) --userad
GO

INSERT [dbo].[Student] ([student_id], [first_name], [last_name]) VALUES (CAST(1 AS Numeric(7, 0)), 'Alex', 'Dionne')
GO
INSERT [dbo].[Student] ([student_id], [first_name], [last_name]) VALUES (CAST(2 AS Numeric(7, 0)), 'Nahom', 'Haile')
GO
INSERT [dbo].[Student] ([student_id], [first_name], [last_name]) VALUES (CAST(26231 AS Numeric(7, 0)), 'David', 'Brennan')
GO

SET IDENTITY_INSERT [dbo].[Language] ON 
GO
INSERT [dbo].[Language] ([language_id], [language_name]) VALUES (CAST(1 AS Numeric(7, 0)), N'Python')
GO
INSERT [dbo].[Language] ([language_id], [language_name]) VALUES (CAST(2 AS Numeric(7, 0)), N'Javascript')
GO
SET IDENTITY_INSERT [dbo].[Language] OFF
GO
DBCC CHECKIDENT(Language)
GO

SET IDENTITY_INSERT [dbo].[DataType] ON 
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(1 AS Numeric(7, 0)), N'str', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(2 AS Numeric(7, 0)), N'int', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(3 AS Numeric(7, 0)), N'float', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(4 AS Numeric(7, 0)), N'complex', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(5 AS Numeric(7, 0)), N'list', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(6 AS Numeric(7, 0)), N'tuple', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(7 AS Numeric(7, 0)), N'range', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(8 AS Numeric(7, 0)), N'dict', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(9 AS Numeric(7, 0)), N'set', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(10 AS Numeric(7, 0)), N'frozenset', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(11 AS Numeric(7, 0)), N'bool', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(12 AS Numeric(7, 0)), N'bytes', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(13 AS Numeric(7, 0)), N'bytearray', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(14 AS Numeric(7, 0)), N'memoryview', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(15 AS Numeric(7, 0)), N'object (PY)', CAST(1 AS Numeric(7, 0)))
GO
SET IDENTITY_INSERT [dbo].[DataType] OFF
GO
DBCC CHECKIDENT(DataType)
GO

SET IDENTITY_INSERT [dbo].[DataType] ON 
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(16 AS Numeric(7, 0)), N'string', CAST(2 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(17 AS Numeric(7, 0)), N'number', CAST(2 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(18 AS Numeric(7, 0)), N'bigInt', CAST(2 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(19 AS Numeric(7, 0)), N'boolean', CAST(2 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(20 AS Numeric(7, 0)), N'array', CAST(2 AS Numeric(7, 0)))
GO
INSERT [dbo].[DataType] ([data_type_id], [data_type_name], [language_id]) VALUES (CAST(21 AS Numeric(7, 0)), N'object (JS)', CAST(2 AS Numeric(7, 0)))
GO
SET IDENTITY_INSERT [dbo].[DataType] OFF
GO
DBCC CHECKIDENT(DataType)
GO

SET IDENTITY_INSERT [dbo].[Challenge] ON 
GO
INSERT [dbo].[Challenge] ([challenge_id], [title], [description], [example], [function_name], [teacher_id], [active], [difficulty_level], [creation_date], [return_type_id]) VALUES (CAST(1 AS Numeric(7, 0)), N'Hello World', N'(Hello world, this desc tests length of average challenge)If we list all the natural numbers below 10 that are multiples of 3 or 5, we get 3, 5, 6 and 9. The sum of these multiples is 23.
Finish the solution so that it returns the sum of all the multiples of 3 or 5 below the number passed in.
Additionally, if the number is negative, return 0 (for languages that do have them). Note: If the number is a multiple of both 3 and 5, only count it once.', N'Kata.AlphabetPosition("The sunset sets at twelve o clock.")', N'hello', CAST(2876 AS Numeric(7, 0)), 1, N'Easy', GETDATE(), CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[Challenge] ([challenge_id], [title], [description], [example], [function_name], [teacher_id], [active], [difficulty_level], [creation_date], [return_type_id]) VALUES (CAST(2 AS Numeric(7, 0)), N'Simple addition', N'Simple addition (2+3)', N'2 + 3', N'big_maths', CAST(2203 AS Numeric(7, 0)), 1, N'Medium', GETDATE(), CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[Challenge] ([challenge_id], [title], [description], [example], [function_name], [teacher_id], [active], [difficulty_level], [creation_date], [return_type_id]) VALUES (CAST(3 AS Numeric(7, 0)), N'Simple subtraction', N'Simple subtraction  (subtract 2 numbers)', N'num1 - num2', N'bigger_maths', CAST(2203 AS Numeric(7, 0)), 1, N'Hard', GETDATE(), CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[Challenge] ([challenge_id], [title], [description], [example], [function_name], [teacher_id], [active], [difficulty_level], [creation_date], [return_type_id]) VALUES (CAST(4 AS Numeric(7, 0)), N'Simple muptiplication', N'Simple muptiplication', N'2*3', N'big_maths', CAST(2203 AS Numeric(7, 0)), 1, N'Easy', GETDATE(), CAST(16 AS Numeric(7, 0)))
GO
SET IDENTITY_INSERT [dbo].[Challenge] OFF
GO
DBCC CHECKIDENT(Challenge)
GO


SET IDENTITY_INSERT [dbo].[ChallengeLanguage] ON 
GO
INSERT [dbo].[ChallengeLanguage] ([challenge_language_id], [language_id], [challenge_id]) VALUES (CAST(1 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[ChallengeLanguage] ([challenge_language_id], [language_id], [challenge_id]) VALUES (CAST(2 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)))
GO
INSERT [dbo].[ChallengeLanguage] ([challenge_language_id], [language_id], [challenge_id]) VALUES (CAST(3 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)), CAST(3 AS Numeric(7, 0)))
GO
INSERT [dbo].[ChallengeLanguage] ([challenge_language_id], [language_id], [challenge_id]) VALUES (CAST(4 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)), CAST(4 AS Numeric(7, 0)))
GO
SET IDENTITY_INSERT [dbo].[ChallengeLanguage] OFF
GO
DBCC CHECKIDENT([ChallengeLanguage])
GO


SET IDENTITY_INSERT [dbo].[CodeSubmission] ON 
GO
INSERT [dbo].[CodeSubmission] ([code_submission_id], [code], [student_id], [challenge_language_id], [status], [last_attempted]) VALUES (CAST(1 AS Numeric(7, 0)), N'def hello_world(): return "Hello World"', CAST(1 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)), CAST(0 AS bit), GETDATE())
GO
INSERT [dbo].[CodeSubmission] ([code_submission_id], [code], [student_id], [challenge_language_id], [status], [last_attempted]) VALUES (CAST(2 AS Numeric(7, 0)), N'def big_maths(): return 2 + 3', CAST(26231 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)), CAST(1 AS bit), GETDATE())
GO
INSERT [dbo].[CodeSubmission] ([code_submission_id], [code], [student_id], [challenge_language_id], [status], [last_attempted]) VALUES (CAST(3 AS Numeric(7, 0)), N'def bigger_maths(num1, num2):'' + CHAR(13)+CHAR(10) + ''return num1 - num2', CAST(26231 AS Numeric(7, 0)), CAST(3 AS Numeric(7, 0)), CAST(1 AS bit), GETDATE())
GO
INSERT [dbo].[CodeSubmission] ([code_submission_id], [code], [student_id], [challenge_language_id], [status], [last_attempted]) VALUES (CAST(4 AS Numeric(7, 0)), N'def bigger_maths(num1, num2): return num1 + num2', CAST(26231 AS Numeric(7, 0)), CAST(3 AS Numeric(7, 0)), CAST(0 AS bit), GETDATE())
GO
INSERT [dbo].[CodeSubmission] ([code_submission_id], [code], [student_id], [challenge_language_id], [status], [last_attempted]) VALUES (CAST(5 AS Numeric(7, 0)), N'def big_maths(): return 2 + 3', CAST(1 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)), CAST(1 AS bit), GETDATE())
GO
INSERT [dbo].[CodeSubmission] ([code_submission_id], [code], [student_id], [challenge_language_id], [status], [last_attempted]) VALUES (CAST(6 AS Numeric(7, 0)), N'def big_maths(): return 2 - 3', CAST(26231 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)), CAST(1 AS bit), GETDATE())
GO
SET IDENTITY_INSERT [dbo].[CodeSubmission] OFF
GO
DBCC CHECKIDENT(CodeSubmission)
GO


SET IDENTITY_INSERT [dbo].[TestCase] ON 
GO
INSERT [dbo].[TestCase] ([test_case_id], [expected_result], [challenge_language_id]) VALUES (CAST(1 AS Numeric(7, 0)), N'Hello world', CAST(1 AS Numeric(7, 0)))
GO
INSERT [dbo].[TestCase] ([test_case_id], [expected_result], [challenge_language_id]) VALUES (CAST(2 AS Numeric(7, 0)), N'5', CAST(2 AS Numeric(7, 0)))
GO
INSERT [dbo].[TestCase] ([test_case_id], [expected_result], [challenge_language_id]) VALUES (CAST(3 AS Numeric(7, 0)), N'4', CAST(3 AS Numeric(7, 0)))
GO
SET IDENTITY_INSERT [dbo].[TestCase] OFF
GO
DBCC CHECKIDENT(TestCase)
GO

SET IDENTITY_INSERT [dbo].[Result] ON 
GO
INSERT [dbo].[Result] ([result_id], [passed], [test_case_id], [code_submission_id], [code_output]) VALUES (CAST(1 AS Numeric(7, 0)), 1, CAST(2 AS Numeric(7, 0)), CAST(5 AS Numeric(7, 0)), N'5')
GO
INSERT [dbo].[Result] ([result_id], [passed], [test_case_id], [code_submission_id], [code_output]) VALUES (CAST(2 AS Numeric(7, 0)), 0, CAST(2 AS Numeric(7, 0)), CAST(6 AS Numeric(7, 0)), N'6')
GO
SET IDENTITY_INSERT [dbo].[Result] OFF
GO
DBCC CHECKIDENT(Result)
GO

SET IDENTITY_INSERT [dbo].[Parameter] ON 
GO
INSERT [dbo].[Parameter] ([parameter_id], [data_type_id], [challenge_language_id], [position], [default_value], [parameter_name]) VALUES (CAST(1 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)), CAST(1 AS Numeric(2, 0)), N'2', "Param1")
GO
INSERT [dbo].[Parameter] ([parameter_id], [data_type_id], [challenge_language_id], [position], [default_value], [parameter_name]) VALUES (CAST(2 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)), CAST(2 AS Numeric(2, 0)), N'abc123', "Param2")
GO
SET IDENTITY_INSERT [dbo].[Parameter] OFF
GO
DBCC CHECKIDENT(Parameter)
GO

SET IDENTITY_INSERT [dbo].[TestCaseParameter] ON
GO
INSERT [dbo].[TestCaseParameter] ([test_case_parameter_id], [test_case_id], [parameter_id], [value]) VALUES (CAST(1 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)), "a")
INSERT [dbo].[TestCaseParameter] ([test_case_parameter_id], [test_case_id], [parameter_id], [value]) VALUES (CAST(2 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)), "100")
INSERT [dbo].[TestCaseParameter] ([test_case_parameter_id], [test_case_id], [parameter_id], [value]) VALUES (CAST(3 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)), CAST(1 AS Numeric(7, 0)), "b")
INSERT [dbo].[TestCaseParameter] ([test_case_parameter_id], [test_case_id], [parameter_id], [value]) VALUES (CAST(4 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)), CAST(2 AS Numeric(7, 0)), "200")
GO
SET IDENTITY_INSERT [dbo].[TestCaseParameter] OFF
GO
DBCC CHECKIDENT(TestCaseParameter)
