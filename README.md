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
- For both titles and descriptions, start with the words "Updated", "Added", "Removed" when applicable.
- Only create a pull-request from *dev* to *master* when a new, valuable feature or hotfix is ready for deployment.