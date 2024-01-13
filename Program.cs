
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EFpro;
using var db = new BloggingContext();

// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

List<User> userlist = new List<User>();
using (var reader = new StreamReader(@"C:\Users\stoff\source\repos\EFpro\Data\Users.csv"))
{
	
	                var header = reader.ReadLine();

	while(!reader.EndOfStream)
	{
		  var line = reader.ReadLine();

		var linesplit = line.Split(',');

		var user = new User
		{
			UserId = int.Parse(linesplit[0]),
			Username = linesplit[1],
			Password = linesplit[2]
		};

		userlist.Add(user);

	}	
}

using (var context = new BloggingContext())
{	    context.Users.RemoveRange(context.Users);
    		context.SaveChanges();
	context.Users.AddRange(userlist);
	context.SaveChanges();
}

using (var context = new BloggingContext())
{
	var users = context.Users.ToList();
	foreach(var user in userlist)
	{
		Console.WriteLine($"UserID: {user.UserId}, Username: {user.Username}");
	}


}

