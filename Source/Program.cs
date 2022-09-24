using Rune;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Bind(new IPEndPoint(IPAddress.Loopback, 11254));
socket.Listen();
Socket client = socket.Accept();

Console.WriteLine("- Connection Accepted");

bool run = true;
while (run)
{
	byte[] buffer = new byte[2048];
	int len = client.Receive(buffer);
	string input = Encoding.UTF8.GetString(buffer);

	Console.WriteLine("- " + input);

	int firstSpaceIndex = input.IndexOf(" ");
	string command = input.Substring(0, firstSpaceIndex);
	string command_args = input.Substring(firstSpaceIndex + 1);

	switch (command)
	{
		case "getbestmove":
			string[] tokens = command_args.Split(' ');
			string fen = tokens[0];
			string turn = tokens[1];
			int colorToMove = turn == "w" ? Piece.White : Piece.Black;
			Board board = Board.FromFen(fen);
			board.Print();
			Move bestMove = Engine.GetBestMove(board, colorToMove);
			board.Print();
			client.Send(Encoding.UTF8.GetBytes(MoveString.FromMove(bestMove)));
			break;
	}
}