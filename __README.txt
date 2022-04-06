To play with this code use VS 2015+.

This code uses new C# 6.0 features like:
- Exception filters using "when"
- String interpolation: $"My number is {_number}"
- Null-conditional operator:   _cancellationTokenSource?.Cancel()

Changelog:
June 2, 2015: Fixed bug stackoverflow on disposing
June 6, 2015: Changed from .NET 4.6 to 4.5	 	 
June 6, 2015: Removed test setting EventQueueSize=1
July 7, 2015: Reenabled auto recovering from FSW errors in BufferingFileSystemWatcher_Error.
