# CryptoTrader
A modular C# bot to analyze cryptomarket trends and trade cryptocurrency, with native support for Binance.

# Development Conventions
- Limit your commits to a per-implementation basis. Commiting smaller changes prevents reviewers from getting lost in the code.
- Titles should contain the encompassing effect of the commit OR the most important element (if the two are not the same, although they should be).
- Only commit once the implementation has been thoroughly tested.
- Ensure the code is either modular in itself or follows the modularity of pre-existing code.
- Every line in the commit description should follow the following format (only include line number(s) if this is a code change or bug fix):
```
- [<FILE> <LINES>] <INFO>
```
- For both titles and descriptions, start with the words "Updated", "Added", "Removed" when applicable AND end every line with a period.
- Create a pull request from *dev* to *master* at some point in your development. The earlier, the better.
- Pull requests should be tailored for the user's understanding. Avoid technicalities and jargon when possible.
- Pull requests should have an assignee (yourself), label, and project before being created. The reviewer can be set later.
- Add bugs/issues using the [Issues Tracker](https://github.com/itsmistad/CryptoTrader/issues). They'll automatically be added to the [To-do column](https://github.com/itsmistad/CryptoTrader/projects/1).
- Track the progress of your pull requests in the [board](https://github.com/itsmistad/CryptoTrader/projects/1). Done and Graveyard columns are automated.