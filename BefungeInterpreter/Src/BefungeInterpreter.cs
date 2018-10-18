using System;
using System.Collections.Generic;
using System.Linq;

namespace BefungeInterpreter {
	public class BefungeInterpreter {

		private enum Direction {
			North, South, West, East
		}
		
		private struct Point {
			public int X { get; set; }
			public int Y { get; set; }
		}

		private readonly Stack<int> stack;
		private readonly int[,] program;
		private Direction direction;
		private Point position;
		private bool stringMode;

		public BefungeInterpreter(string programRaw) {
			var lines = programRaw.Split('\n');
			int lineAmount = lines.Length, longestLine = 0;
			longestLine = lines.Select(line => line.Length).Concat(new[] {longestLine}).Max();
			
			program = new int[longestLine, lineAmount];

			for(var i = 0; i < lines.Length; i++){
				var line = lines[i].TrimEnd().ToCharArray();
				for(var j = 0; j < line.Length; j++){
					program[j, i] = line[j];
				}
			}
			
			stack = new Stack<int>();
			direction = Direction.East;
			position = new Point {
				X = 0,
				Y = 0
			};
			stringMode = false;
		}

		public void Execute() {
			while(program[position.X, position.Y] != '@'){
				Eval();
				Next();
			}
			
			direction = Direction.East;
			position.X = 0;
			position.Y = 0;
			stringMode = false;
			stack.Clear();
		}

		private void Eval() {
			var c = (char) program[position.X, position.Y];
			if(stringMode && c != '"')
				Push(c);
			else{
				if(char.IsDigit(c))
					Push(c - '0');
				else {
					switch(c){
						case ' ':
							Empty();
							break;
						
						case '+':
							Add();
							break;
						case '-':
							Subtract();
							break;
						case '*':
							Multiply();
							break;
						case '/':
							Divide();
							break;
						case '%':
							Mod();
							break;

						case '!':
							Not();
							break;
						case '`':
							GreaterThan();
							break;

						case '<':
							West();
							break;
						case '>':
							East();
							break;
						case '^':
							North();
							break;
						case 'v':
							South();
							break;
						case '?':
							RandomDir();
							break;

						case '_':
							IfHorizontal();
							break;
						case '|':
							IfVertical();
							break;

						case '"':
							SwitchStringMode();
							break;

						case ':':
							Duplicate();
							break;
						case '\\':
							Switch();
							break;
						case '$':
							Pop();
							break;

						case '.':
							PrintNum();
							break;
						case ',':
							PrintChar();
							break;

						case '#':
							Next();
							break;

						case 'g':
							GetChar();
							break;
						case 'p':
							PutChar();
							break;

						case '&':
							ReadInt();
							break;
						case '~':
							ReadChar();
							break;
					}
				}

			}
			void Empty(){}
		}

		private int Peek() {
			return stack.Peek();
		}
		private int Pop() {
			return stack.Pop();
		}
		private void Push(int n) {
			stack.Push(n);
		}

		private void Next() {
			switch(direction){
				case Direction.East:
					position.X++;
					break;
				case Direction.West:
					position.X--;
					break;
				case Direction.North:
					position.Y--;
					break;
				case Direction.South:
					position.Y++;
					break;
			}
		}

		private void Add() {
			Push(Pop() + Pop());
		}
		private void Subtract() {
			var a = Pop();
			var b = Pop();
			Push(b - a);
		}
		private void Multiply() {
			Push(Pop() * Pop());
		}
		private void Divide() {
			var a = Pop();
			var b = Pop();
			Push(b / a);
		}
		private void Mod() {
			var a = Pop();
			var b = Pop();
			Push(b % a);
		}

		private void Not() {
			Push(Pop() == 0 ? 1 : 0);
		}
		private void GreaterThan() {
			var a = Pop();
			var b = Pop();
			Push(b > a ? 1 : 0);
		}

		private void West() {
			direction = Direction.West;
		}
		private void East() {
			direction = Direction.East;
		}
		private void South() {
			direction = Direction.South;
		}
		private void North() {
			direction = Direction.North;
		}
		private void RandomDir() {
			var rand = new Random();
			direction = new []{Direction.West, Direction.East, Direction.North, Direction.South}[rand.Next(3)];
		}

		private void IfHorizontal() {
			if(stack.Pop() == 0)
				East();
			else
				West();
		}
		private void IfVertical() {
			if(stack.Pop() == 0)
				South();
			else
				North();
		}

		private void SwitchStringMode() {
			stringMode = !stringMode;
		}

		private void Duplicate() {
			Push(Peek());
		}
		private void Switch() {
			var a = Pop();
			var b = Pop();
			Push(a);
			Push(b);
		}

		private void PrintNum() {
			Console.Write(Pop());
		}
		private void PrintChar() {
			Console.Write((char) Pop());
		}

		private void PutChar() {
			var y = Pop();
			var x = Pop();
			var v = Pop();
			program[x, y] = v;

		}
		private void GetChar() {
			var y = Pop();
			var x = Pop();
			Push(program[x,y]);
		}
		
		private void ReadInt() {
			var a = Console.ReadLine();
			if(a != null)
				Push(int.Parse(a));
		}
		private void ReadChar() {
			var a = Console.ReadLine();
			if(a != null)
				Push(char.Parse(a));
		}
		
	}
}