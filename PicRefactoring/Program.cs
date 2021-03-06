﻿using System;
using PicRefactoring.Abstractions;
using static System.Console;

namespace PicRefactoring
{
	public class Program
	{
		static void Main(string[] args)
		{
			var commands = ReadCommands(args);
			var executor = new CommandsExecutor(commands);
			executor.ExecuteCommands();
			ReadKey();
		}

		private static ICommands ReadCommands(string[] args)
		{
			var commandParser = new CommandsParser();
			try
			{
				commandParser.OpenCommandsFile(args);
				return commandParser.ParseModel();
			}
			catch (Exception e)
			{
				WriteLine(e);
				ReadKey();
				throw;
			}
		}
	}
}
