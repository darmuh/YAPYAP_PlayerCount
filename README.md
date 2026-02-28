# PlayerCount (YAPYAP)

### Host only mod that allows for modifying the player lobby limit up to 20 players (or down to 2!)

- This is an incredibly simple, lightweight mod that just modifies the lobby player limit from the default.
- The limit is fully configurable and can both increase the limit up to 20 players or decrease the limit down to 2 players.
- **Use at your own risk and remember that increasing this player limit has the potential to introduce unintended bugs.**

### Configuration Options

 - ``Max Players`` Set desired maximum number of players
 - ``Toggle UI Hint`` When host of a lobby, this key can be used to toggle the in-game UI hint off/on (top left)
 - ``Spawn Extra Starter Wands`` When enabled, will spawn extra starter wands in the lost and found box.
	- The amount of extra wands to spawn is determined by how many extra slots your lobby has (in comparison to vanilla)  
	- So a lobby with 10 slots will spawn 4 extra starter wands in the lost and found box (10 slots - 6 vanilla slots)  
	- **Extra wands will only spawn in the lobby on the first night of a save.**  

### In-game configuration
 - As of 0.2.0, there is now an in-game setting located in Settings -> General labeled ``MAX PLAYERS``  
	- This setting can be changed at any time. However, for the value change to take affect you will need to host a new lobby.  
 - If [YapLocalizer](https://thunderstore.io/c/yapyap/p/darmuh/YapLocalizer/) is present, the Max Players setting will have added localizations.
	- If a localization does not exist for your language or could be improved please let me know!

### Quota Scaling by Player Count (QuotaQueen by Robyn)  
 - If you have [QuotaQueen](https://thunderstore.io/c/yapyap/p/Robyn/QuotaQueen/) installed, this mod will add two Quota Stratgey configuration options to QuotaQueen's configuration.    
	- ``PlayerCount.ScaleUp`` - When the player count is over the vanilla maximum ``(6)``, the quota will be scaled up by the current player count.    
		- Example: If the quota is ``1800`` and there are 8 players, the quota will be converted using the following formula: ``1800(8/6) = 2400``  
		- The quota will NOT be modified when there is ``6`` or less players.  
	- ``PlayerCount.ScaleAlways`` - The quota will always be scaled by the number of current players compared to the vanilla maximum.  
		- Example: If the quota is ``1800`` and there are 4 players, the quota will be converted using the following formula: ``1800(4/6) = 1200``  
		- The quota will **always** be modified unless there are exactly 6 players.  
	- You will find these options under the ``Quota Strategy`` configuration item in ``Quota Settings``