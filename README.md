# PlayerCount (YAPYAP)

### Host only mod that allows for modifying the player lobby limit up to 20 players (or down to 2!)

- This is an incredibly simple, lightweight mod that just modifies the lobby player limit from the default.
- The limit is fully configurable and can both increase the limit up to 20 players or decrease the limit down to 2 players.
- **Use at your own risk and remember that increasing this player limit has the potential to introduce unintended bugs.**

### Configuration Options

 - ``Max Players`` Set desired maximum number of players

### Quota Scaling by Player Count (QuotaQueen by Robyn)  
 - If you have [QuotaQueen](https://thunderstore.io/c/yapyap/p/Robyn/QuotaQueen/) installed, this mod will add two Quota Stratgey configuration options to QuotaQueen's configuration.    
	- ``PlayerCount.ScaleUp`` - When the player count is over the vanilla maximum ``(6)``, the quota will be scaled up by the current player count.    
		- Example: If the quota is ``1800`` and there are 8 players, the quota will be converted using the following formula: ``1800(8/6) = 2400``  
		- The quota will NOT be modified when there is ``6`` or less players.  
	- ``PlayerCount.ScaleAlways`` - The quota will always be scaled by the number of current players compared to the vanilla maximum.  
		- Example: If the quota is ``1800`` and there are 4 players, the quota will be converted using the following formula: ``1800(4/6) = 1200``  
		- The quota will **always** be modified unless there are exactly 6 players.  
	- You will find these options under the ``Quota Strategy`` configuration item in ``Quota Settings``