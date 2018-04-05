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

### Custom drawing offsets for riders (Only on master branch, not released yet)

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
### Custom allowed life stages (Only on master branch, not released yet)

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
### Restrict used mount per faction.

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
