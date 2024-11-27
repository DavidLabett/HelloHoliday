# HelloHoliday

## Rules
- **We use English in all our files, code and comments**
- **We do our best to use the standard naming conventions in our code**
- **We name things in a way so that we can read what it is**

## Git Rules
- **We make sure that the main branch is clean and up to date with working code**
- **We work from dev as a safety measure**
- **We use lowercase when we create a new branch**
- **We recommend using tab to autocomplete branch-names when moving between them (`git checkout <branch-name>`) or doing a merge (`git merge <branch-name>`) to ensure that we spell them correctly**
- **We create branches for features and submenus**
- **We do not continue in a feature branch that has completed its purpose**
- **All feature branches start with `feature/<name>`**
- **If we are working together on a branch we communicate when we push changes to that branch**
- **All merges need to be communicated to the team so that no changes to the shared branches happen without prior knowledge (main-branch and dev-branch)**
- **We always merge first with our feature branch and handle all potential conflicts in our feature branch (or if we want to merge dev with main, we first merge main to dev)**
- **We only merge with dev when all conflicts are resolved**
- **We only merge with main when all conflicts are resolved, and we have tested and working code**

## Menu Rules
- **When creating a feature branch for a menu we write `feature/menu/<name>` (ex. `feature/menu/customer`)**
- **When we create a feature branch for a submenu we write `feature/menu/<name>/<subname>` (ex. `feature/menu/customer/mybookings`)**
- **Class names should be named so the first word is the type of menu, and the second word is “menu” (ex. `BookingMenu` or `CustomerMenu`)**


# Structure
### Enums
**Table:room -> Column:size**
`'Single','Double','Triple','Quad'`

**Table:rating -> Column:rating**
`'*','**','***','****','*****'`


# HOW TO USE GIT

### Important Notice
- **Make sure to commit a .gitignore file when creating your repo:**  
  `https://gist.github.com/drch-/00d98ab2b93b930f55e0f7c62abc4af2`

### 1. Basic Commands
- **Clone existing repo:**
  `git clone <repository_ssh>`

- **Check the status:**  
  `git status`

- **Check commit history:**  
  `git log` (Press q to exit log mode)

- **Show current branch:**  
  `git branch`

- **Show all branches:**  
  `git branch -a`

  ---

### 2. Working with Branches
- **Create a new branch:**  
  `git branch <branch_name>`

- **First push inside new branch:**
  `git push --set-upstream origin <branch_name>`

- **Switch to another branch:**  
  `git checkout <branch_name>`

- **Shortcut: Create and switch to a new branch:**  
  `git checkout -b <branch_name>`

- **Delete a branch (local):**  
  `git branch -d <branch_name>`  
  *(Use `-D` to force delete without checking if it's merged, and discarding changes)*

- **Change working branch**  
  `git checkout <branch_name>`

- **Checkout from all branches/reset**  
  `git checkout *`

- **View all branches**  
  `git branch -a`

  ---

### 3. Adding, Committing, and Pushing Changes
- **Add changes to staging (specific file):**  
  `git add <file_name>`

- **Shortcut: Add + Commit in the same command:**  
  `git commit -am "Your commit message"`
  **(Cannot be used when adding new files)**

    - **Add all changes to staging:**  
      `git add .`

    - **Commit changes:**  
      `git commit -m "Your commit message"`

- **Push changes (in current branch):**  
  `git push`

- **Pull the latest changes from remote repo:**  
  `git pull`

- **Downloads the state of new data from a remote repository, but it **does not automatically merge** changes into your current working directory. It allows you to see what changes are available on the remote before deciding to merge them with your local branch.**
    - `git fetch`

  ---

### 4. Merging Changes

1. **Make sure both branches have a clean head:**
   `git status`
   **For each branch**

2. **Checkout the branch you are working on**
   `git checkout <branch_name>`

3. **Merge main into your current branch:**  
   `git merge main`

*(Merges the specified branch (main) into the branch you're currently on.)*

4. **Resolve merge conflicts:**  
   When conflicts occur during a merge, Git will mark the files with conflicts. You’ll need to edit the files manually, remove conflict markers, and then **add** and **commit** the resolved files:
   `git add <file_name>`
   `git commit`

5. **Test the code for errors**

6. **After conflicts are respolved, and code is tested:**  
   `git push`

7. **Now checkout main and merge your working branch into main**
   `git checkout main`
   `git merge <branch_name>`
   `git push`
   **done**

  ---

### 5. Reset & undo
- **If a file is unstaged -> Undo unstaged changes in file:**  
  `git checkout <file_name>`

- **If a file is staged -> Reset file to last committed state**  
  `git reset <file_name>`

- **Reset all changes (in branch) to last committed state**  
  `git reset .`

- **Checkout from all branches/reset**  
  `git checkout *`
  

