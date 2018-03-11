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

# Configuring draw priority and forbidden animals 

In the mod options animals can be forbidden to be used as mount and the drawing priority of mounted animals can be configured. If you want to have this preconfigured for the animals in a mod you've developed, a patch can be created. 
Check out Patches/Races.xml for an example. 

# Restrict used mount per faction. 

If you want a certain faction only to use certain mounts, you can create a patch like the following example. You can also directly add a <modExtensions> tag to a FactionDef instead of patching. 
In the example, the Pirate faction is patched so that it only uses Muffalos and Cougars. Two patches are used to ensure it also works when the faction already has a modExtensions tag. 
 
```xml
	<Operation Class="PatchOperationSequence">
		<success>Always</success>
		<operations>
		<li Class="PatchOperationAdd">
			<xpath>*/FactionDef[ defName = "Pirate"]/modExtensions</xpath> 
			<value>
				<modExtensions>
					<li Class="GiddyUpCore.FactionRestrictionsPatch">
						<mountChance>30</mountChance>
						<wildAnimalWeight>100</wildAnimalWeight> <!--Weights can have any integer value, and the relative fraction to the other weight will determine the change a type of animal spawns-->
						<domesticAnimalWeight>0</domesticAnimalWeight> <!-- setting this to 0 ensures no default domestic animals are spawned -->
						<allowedWildAnimalsCSV>Muffalo,Cougar</allowedWildAnimalsCSV> <!--Use a csv with animal DefNames-->
						<allowedDomesticAnimalsCSV>""</allowedDomesticAnimalsCSV> <!-- only making this empty will imply no restrictions at all, so make sure domesticAnimalWeight is 0 if you don't want any domestic animals-->
					</li>
				</modExtensions>
			</value>
		</li>
		</operations>
	</Operation>
	
	<Operation Class="PatchOperationSequence">
		<success>Always</success>
		<operations>
		<li Class="PatchOperationAdd">
			<xpath>*/FactionDef[ defName = "Pirate" and not(DefModExtension)]</xpath>
			<value>
				<modExtensions>
					<li Class="GiddyUpCore.FactionRestrictionsPatch">
						<mountChance>30</mountChance>
						<wildAnimalWeight>100</wildAnimalWeight> <!--Weights can have any integer value, and the relative fraction to the other weight will determine the change a type of animal spawns-->
						<domesticAnimalWeight>0</domesticAnimalWeight> <!-- setting this to 0 ensures no default domestic animals are spawned -->
						<allowedWildAnimalsCSV>Muffalo,Cougar</allowedWildAnimalsCSV> <!--Use a csv with animal DefNames-->
						<allowedDomesticAnimalsCSV>""</allowedDomesticAnimalsCSV> <!-- only making this empty will imply no restrictions at all, so make sure domesticAnimalWeight is 0 if you don't want any domestic animals-->
					</li>
				</modExtensions>
			</value>
		</li>
		</operations>
	</Operation>
```
