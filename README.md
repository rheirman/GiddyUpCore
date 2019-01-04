# Giddy-up! Core

## Install

Download the zip from one of the releases, unpack and place the unpacked folder in the Rimworld mods folder. 

## Contributing

Translations are always very welcome. Just create a pull request.

## Licence
Feel free to add this mod to modpacks or to use the code or icons for other projects. 
Do however not release exact copies of my project, or exact copies with minor adjustments without my consent.
Code is derivate from Ludeon Studio.

## Patching 

No patching is needed to get Giddy-up working for other animal adding mods. However, some configurations can be patched: 

### Configuring draw priority and forbidden animals 

In the mod options animals can be forbidden to be used as mount and the drawing priority of mounted animals can be configured. If you want to have this preconfigured for the animals in a mod you've developed, a patch can be created. 
Check out Patches/Races.xml for an example. 

### Custom drawing offsets for riders

Giddy-up automatically detects the position of the back of the animal to determine where to draw the rider. In some cases it may look better when deviating from this automatically determined position. For this purpose, you can configure custom drawing offsets for each animal individually. This offset is relative to the drawing position determined automatically by the mod. The following example patch ensures riders are drawn on the back of thrumbo's instead of on their neck. 

```xml
<Patch>
	<Operation Class="PatchOperationSequence">
		<success>Always</success>
		<operations>
		<li Class="PatchOperationAddModExtension">
			<xpath>*/ThingDef[ defName = "Thrumbo"]</xpath> 
			<value>
				<li Class="GiddyUpCore.DrawingOffsetPatch">
					<!--Offsets can be configured for each view (north, south, west, east) separately using comma separated floating point values-->
					<northOffsetCSV>0,0,-0.6</northOffsetCSV><!--x,y,z coordinates. Mind that x: horizontal axis, y: drawing priority, z: vertical axis -->	
					<southOffsetCSV>0,0,-0.6</southOffsetCSV>	
					<eastOffsetCSV>-0.5,0,-0.6</eastOffsetCSV>	
					<westOffsetCSV>0.5,0,-0.6</westOffsetCSV>	
				</li>
			</value>
		</li>
		</operations>
	</Operation>
</Patch>
```
### Custom allowed life stages 

By default, only animals in their final life stage can be mounted. However, by patching, you can allow other life stages. The following example patch allows that the Muffalo can be mounted during any life stage:

```xml
<Patch>
	<Operation Class="PatchOperationSequence">
		<success>Always</success>
		<operations>
		<li Class="PatchOperationAddModExtension">
			<xpath>*/ThingDef[ defName = "Muffalo"]</xpath> 
			<value>
				<li Class="GiddyUpCore.AllowedLifeStagesPatch">
					<allowedLifeStagesCSV>0,1,2</allowedLifeStagesCSV> <!-- Provide the life stage indices as csv here.-->	
				</li>
			</value>
		</li>
		</operations>
	</Operation>
</Patch>
```
### Restrict mount usage per faction.

If you want a certain faction only to use certain mounts, you can create a patch like the following example. You can also directly add a <modExtensions> tag to a FactionDef instead of patching. 
In the example, the Pirate faction is patched so that it only uses Muffalos and Cougars.
 
```xml
<Patch>
	<Operation Class="PatchOperationSequence">
		<success>Always</success>
		<operations>
		<li Class="PatchOperationAddModExtension">
			<xpath>*/FactionDef[ defName = "Pirate"]</xpath> 
			<value>
				<li Class="GiddyUpCore.FactionRestrictionsPatch">
					<mountChance>30</mountChance>
					<!-- wild animals are the animals that can spawn in the wild -->
					<wildAnimalWeight>100</wildAnimalWeight> <!--Weights can have any integer value, and the relative fraction to the other weight will determine the change a type of animal spawns-->
					<nonWildAnimalWeight>0</nonWildAnimalWeight> <!-- setting this to 0 ensures no default domestic animals are spawned -->
					<!-- nonWild animals are the animals that cannot spawn in the wild, examples are Thrumbo's, farm animals etc.  -->
					<allowedWildAnimalsCSV>Muffalo,Cougar</allowedWildAnimalsCSV> <!--Use a csv with animal DefNames-->
					<allowedNonWildAnimalsCSV>""</allowedNonWildAnimalsCSV> <!-- only making this empty will imply no restrictions at all, so make sure domesticAnimalWeight is 0 if you don't want any domestic animals-->
				</li>
			</value>
		</li>
		</operations>
	</Operation>
</Patch>
```
### Restrict mount usage per PawnKind. 

It's also possible to restrict the mount usage of certain PawnKinds. In this example, the MercenarySlasher PawnKind, is patched so it only uses muffalo's and thrumbos as mount (with a high thrumbo chance :)). 

```xml
<Patch>
	<Operation Class="PatchOperationSequence">
		<success>Always</success>
		<operations>
		<li Class="PatchOperationAddModExtension">
			<xpath>*/PawnKindDef[ defName = "MercenarySlasher"]</xpath>
			<value>
				<li Class="GiddyUpCore.CustomMountsPatch">
					<mountChance>100</mountChance><!-- % chance the pawn of the PawnKind will be a rider with a mount-->
					<possibleMounts>
						<li>
							<key>Muffalo</key> <!-- DefName of the mount-->
							<value>10</value> <!-- Weight determining the chance the mount will be of the type of the key --> 
						</li>
						<li>
							<key>Thrumbo</key>
							<value>200</value><!-- note that the weights don't have to add up to 100, as they are normalized automatically -->
						</li>
					</possibleMounts>
				</li>
			</value>
		</li>
		</operations>
	</Operation>
</Patch>
	
```
### Using overlay textures to make some mounts look good.

Some mounts, especially the ones with long horns or long necks, look strange when the pawn is drawn in front, or behind them, because the pawn either overlaps strange parts of the animal, or drawing the pawn behind the animal doesn't look appropriate. An example of such an animal is the caribou. By patching, it's possible to solve this problem.

By default, a mounted caribou would look like this: 
![Image of Caribou - problematic](https://i.imgur.com/zYVXxLE.jpg)

However, when patched properly it looks like this: 
![Image of Caribou - patched](https://i.imgur.com/KElb0NV.jpg)

To achieve this, the parts of the animal that should overlap the rider are provided as images, and the following patch is used:
```xml
<Patch>
	<Operation Class="PatchOperationSequence">
		<success>Always</success>
		<operations>
		<li Class="PatchOperationAdd">
			<xpath>*/ThingDef[defName = "Caribou"]/comps</xpath> <!-- Note that the Caribou always has comps. some animals don't have comps by default though. Make sure to take this into account when patching. -->
			<value>
				<li Class="GiddyUpCore.CompProperties_Overlay">
					<overlayFront> <!--Different overlays can be provided for different viewpoints, possibilities are: overlayFront, overlaySide, and overlayBack -->
						<graphicDataDefault><!-- Different graphics data can be provided for different genders, possibilities are: graphicDataDefault(for all genders), graphicsDataMale and graphicsDataFemale -->
						  <texPath>Things/Pawn/Caribou_overlay_front</texPath>
						  <graphicClass>Graphic_Single</graphicClass>
						  <drawSize>4.38</drawSize>
						  <drawRotated>false</drawRotated>
						  <shaderType>MetaOverlay</shaderType>
						</graphicDataDefault>					
					    <offsetDefault>(0,0,0,0)</offsetDefault> <!--  Different offsets can be provided for different genders. Possibilities are: offsetDefault(for all genders), offsetFemale and offsetMale --> 
					</overlayFront>
					<overlaySide>
						<graphicDataDefault>
						  <texPath>Things/Pawn/Caribou_overlay_side</texPath>
						  <graphicClass>Graphic_Single</graphicClass>
						  <drawSize>4.38</drawSize>
						  <drawRotated>false</drawRotated>
	                                          <shaderType>MetaOverlay</shaderType>
						</graphicDataDefault>
					    <offsetDefault>(0,0,0,0)</offsetDefault>
					</overlaySide>
				</li>
			</value>
		</li>
		</operations>
	</Operation>
</Patch>
```

### Custom stats when animals are mounted. 

Want to make an animal faster when it's mounted? Or want it to have more armor? Then this patching option is what you need. In the following example, a Thrumbo is patched so it's twice as fast,  has a rediculous amount of armor when mounted, and has a metal bounce off effect when the armor deflects projectiles:
```xml
<Operation Class="PatchOperationAddModExtension">
	<xpath>*/ThingDef[ defName = "Thrumbo"]</xpath> 
	<value>
		<li Class="GiddyUpCore.CustomStatsPatch">
			<!--Speed factor. In this example the thrumbo is made twice as fast when used as mount -->
			<speedModifier>2.0</speedModifier>
			<!--Armor factor. In this example the thrumbo has 10 times the heat/blunt/sharp armor when used as mount, making it completly indistructible -->
			<armorModifier>10.0</armorModifier>
			<!--Setting useMetalArmor to true, makes the animal's skin have the metal bounce of effect when hit, when used as mount --> 
			<useMetalArmor>true</useMetalArmor>
		</li>
	</value>
</Operation>
```
Want more patching possibilities? Just ask!








