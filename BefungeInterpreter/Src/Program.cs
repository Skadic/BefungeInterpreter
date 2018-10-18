using System;
using System.IO;

namespace BefungeInterpreter {
	internal class Program {
		
		public static void Main(string[] args) {
			if(args.Length != 1){
				Console.WriteLine("Please enter the Path to Befunge code (make sure to enclose paths with spaces in quotation marks)");
				return;
			}
			
			var program = File.ReadAllText(args[0]);
			Console.WriteLine(program);
			var interpreter = new BefungeInterpreter(program);
			interpreter.Execute();
		}
	}
}