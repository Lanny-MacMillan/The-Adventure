<div align="center">

# Two Dudes - Evil Frank/Franks Revenge
created by: Two Dudes Games

![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)

</div>


# Two Dudes - Evil Frank/Franks Revenge
Two Dudes Unity Game - Evil Frank/Franks Revenge

## Two Dudes Rules
1. We do not push to Main
2. WE DO NOT PUSH TO MAIN
  - Everyone should only ever be in develop, main is the functional version of the game. All changes will be pushed to develop, a test branch, and then you would PR to main
   
## Pull Repo and Run Github Desktop
1. Install Unity/UnityHub -> https://unity.com/download
2. Install GitHub Desktop -> https://desktop.github.com/
3. Open Github Desktop -> File -> Options -> Accounts -> Sign In
4. After you log in you should be able to see a lst of your git repositories.
   - If Unity_Game_Two_Dudes is not in the list, Go to https://github.com/Lanny-MacMillan/Unity_Game_Two_Dudes and click Code -> Open with       Github Desktop. Doing this from github will clone the repo locally and you _shouldnt_ have to do steps 5-7, follow prompts to clone: <br/>
    <img width="1262" alt="Screen Shot 2023-08-24 at 8 28 05 PM" src="https://github.com/Lanny-MacMillan/Unity_Game_Two_Dudes/assets/102826450/d8570c98-5e79-4cd1-bed2-5ad3b41bc324">
5. Find and click Unity_Game_Two_Dudes and then click "Clone Unity_Game_Two_Dudes"
6. Then choose local path location and click "Clone"
7. You can then open the project using Unity Hub

## Unity Hub
1. Click Projects and find the "Evil Frank/Franks Revenge" from your local path location
2. Click "Evil Frank/Franks Revenge" and Unity Hub should open Unity.

## Starting Game
1. Once Unity Hub has opened Unity, click the Project Tab on the bottom.
   - Assets -> Level -> Scenes -> World Map
     
**NOTE** - The Game will only be playable if you start it at the World Map. The playerChar, playerCharHealth, Inventory UI, and GameOver UI are all created on that initial scene and then carried from scene to scene until death. If you open another scene, it will load and you can style or create but wont be playable, with the playerChar.

## Making Changes to the Game
Before you even start editing or making changes. 
1. Go to GitHub Desktop, set current branch to develop and fetch or pull latest code changes
2. Now that develop has the most recent changes. While in develop click "New Branch" from the "Current Branch list" and make a new feature branch. Label the branch to reflect the changes your making. I usually do something like: "turnBasedBattle/statChanges", "worldMap/enemies", or "bathroom/design". So anyone including yourself can easily see what the branch has, this will help alot as things are added and added.
   - This new branch off develop is local to you and wont interfere with the game as a whole so dont be afraid, if things go bad you can switch back to develop and create another branch and just forget the broken branch ever existed.
3. Make edits
4. If you want to add these edits to the game. Open github Desktop and "current Branch" should be set to your new feature branch from step 2 and not develop or main. Fill out the summary of what the change entails and click "Commit to Main". This message is just a note that can be viewed on GitHub and tell the team on the PR whats been done in the commit history.
5. Next CLick "Push to Origin". This will push staged and commited changes to the selected feature branch.
6. Open GitHub -> Unity_Game_Two_Dudes -> Pull Requests Tab -> New Pull Request
7. Set Compare Feature Branch to Base Develop, Image below:
   <img width="1166" alt="Screen Shot 2023-08-24 at 11 55 12 PM" src="https://github.com/Lanny-MacMillan/Unity_Game_Two_Dudes/assets/102826450/9c4eefe4-78e0-48dc-a826-d17bead83a16">
8. Set a reviewer to approve the change and click submit PR
