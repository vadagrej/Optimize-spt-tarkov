# Optimize-spt-tarkov
quick dll that optimize SPT tarkov
Features
Performance Optimization
Increases game process priority to High for improved CPU scheduling.

Disables performance-heavy graphical features:

Vertical Sync (VSync)
Shadow Cascades
Anti-Aliasing
Pixel-based lighting (pixelLightCount)


Reduces Level of Detail (LOD) bias for distant objects.

Sets maximum frame rate using Application.targetFrameRate = 999.

AI Management
Automatically disables AI (GameObject.SetActive(false)) when bots are located more than 200 meters from the player.

Bot activity is evaluated every 2 seconds to minimize CPU load.

May reduce in-game difficulty when encountering distant AI enemies.

Interface and Diagnostics
Displays an on-screen message "FULL BOOST SAFE ACTIVE" at game startup for confirmation.
