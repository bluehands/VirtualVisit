# Setup after cloning

After cloning:<br>
Reimport the 'Resources' and the 'Virtual Visit' folder inside 'Assets'<br>
If there is a problem with the cardboard sdk, consider the following:<br>
* The Settings for the xr plugin might be bugged after checkout. 
    If that is the case, change the asset serialization from 'force text' to 'mixed' and back to 'force text'. 
    In that process any errors in the configuration should be resolved by Unity automatically