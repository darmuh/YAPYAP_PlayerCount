# Changelog

## 0.2.0
 - Injected mod config setting into settings menu for changing it in-game
	- Added soft dependency to YapLocalizer to add localizations for this setting.
 - Added toggle-able UI indicator of the current lobby's maximum players per request.
	- Keybind to show/hide this is configurable.
	- Non-host players will not see this text ever, since they dont have access to the information as clients.
 - Updated readme to include the new stuff

## 0.1.2
 - Updated to work with latest version of the game (Feb 10th, 2026)
 - Removed Quota Modifier setting in favor of Soft Compatibility with [QuotaQueen](https://thunderstore.io/c/yapyap/p/Robyn/QuotaQueen/)
	- See readme for details

## 0.1.1
 - Updated changelog from the mod base template, lol.
 - Added new setting Quota Modifier, which allows you to modify the quota based on the number of players in the lobby.
	- There are 3 potential settings:
	    - None: No modifications will be applied.
		- OnlyScaleUp: Modifications will only be applied when there is more than 6 players.
		- AlwaysScale: Quota will always be scaled based on the number of players in the lobby, resulting in easier quotas when there is less than 6 players.   

## 0.1.0
 - Initial release.