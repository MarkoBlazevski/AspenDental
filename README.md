🚀 GitHub API and Triangle App Testing Project
This project is dedicated to testing the GitHub API and the Triangle App. Automated tests are executed against various endpoints, ensuring the robustness and reliability of these platforms.

🛠️ Setup and Configuration
Before running the tests, you need to configure the necessary settings to communicate with the GitHub API.

📝 appsettings.json Configuration:
ReportPath: This is the local path where the test execution report will be generated and stored. Update it with the desired location on your machine.

LoginName: This is your GitHub username. Replace this with your GitHub account's username.

BearerToken: For authentication with the GitHub API, you need a personal access token. This token should have the delete_repo scope to allow the testing suite to create and delete repositories.

🚫 WARNING: Always ensure that your BearerToken is kept private. Never expose this token in public repositories or any public platform.

🏁 Running the Tests
After updating the appsettings.json file with the necessary configurations:

Build the solution.
Execute the test suite.
Remember to always check the generated report at the ReportPath you configured!

🎉 Conclusion
We hope this tool serves you well in ensuring the functionality and reliability of the GitHub API and the Triangle App. Happy Testing! 💡

Feel free to make any modifications or adjustments as needed!
