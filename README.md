
# Kizuku (築く) 🇯🇵

## 🎯 About The Project

Kizuku (築く) is a Blazor WebAssembly application designed to optimize the university preparation study process. The name, meaning "to build," "to construct," or "to establish" in Japanese, reflects the project's core purpose: helping users build a strong knowledge foundation 🧠, construct effective study habits ✍️, and establish a system for continuous improvement ✅ by identifying and reducing inefficiencies based on Lean principles. It aims to make studying more structured, measurable, and motivating through gamification elements 🎮.

This tool addresses common challenges in self-directed study, such as lack of structure, difficulty tracking progress, and demotivation caused by hidden inefficiencies. By applying Lean concepts (identifying 'waste' 🗑️) to study sessions and incorporating gamification, Kizuku provides a framework for more focused and productive learning 📈.

## ✨ Key Features

*   **Topic Management:** Create, view, edit, and delete study Topics and Subtopics 📚.
*   **Status Tracking:** Assign progress statuses to topics (`Not Started`, `In Progress`, `Needs Review`, `Completed`, `Repeat`) for clear visibility🚦.
*   **Modularization:** Group related topics into Modules for a high-level overview of subject areas 📁.
*   **Time Tracking:** Track study time accurately with a live start/stop timer ⏱️ or manual entry (including topic, duration, and date) 📅.
*   **Waste Tagging:** Identify study inefficiencies by tagging time logs/sessions with predefined categories (`Technical Difficulties` 💻, `Environmental Distraction` 🌳, `Social Media Distraction` 📱, `Unclear Material`❓, `Lack of Motivation` 📉, `Mental Block/Psyche` 🤔).
*   **Visualization & Gamification:**
    *   View total time spent per topic 📊.
    *   Monitor Module completion with progress bars.
    *   Build and track a daily Study Streak 🔥 for motivation.
    *   (Optional) Analyze inefficiencies with a Waste category chart.

## 💻 Technology Stack

This project utilizes the following technologies:

*   **Backend:** C# / .NET
*   **Frontend:** Blazor WebAssembly
*   **Database:** Entity Framework Core with Sqlite (for development)

## 📸 Screenshots/GIFs

## 🚀 Getting Started / Installation

Follow these steps to set up the project locally:

### Prerequisites

*   .NET SDK 9.0

### Installation

1.  Clone the repository:
    ```
    git clone https://github.com/your_username_/Kizuku.git
    ```
2.  Navigate to the project directory:
    ```
    cd Kizuku
    ```
3.  Restore .NET dependencies:
    ```
    dotnet restore
    ```
4.  Apply database migrations (using EF Core):
    ```
    dotnet ef database update
    ```
    *(Note: Ensure EF Core tools are installed: `dotnet tool install --global dotnet-ef`)*
5.  Run the application:
    ```
    dotnet run
    ```
    The application should now be accessible via your browser, typically at `https://localhost:` or `http://localhost:`.

## 💡 Usage

Once the application is running, you can start optimizing your study process:

1.  **Create Modules:** Define broad subject areas or courses as Modules.
2.  **Add Topics/Subtopics:** Break down Modules into smaller, manageable Topics and Subtopics. Assign initial statuses.
3.  **Track Study Time:**
    *   Use the live timer when starting a study session for a specific topic.
    *   Alternatively, manually log past study sessions, specifying the topic, duration, and date.
4.  **Tag Waste:** During or after a study session, tag the time log if you encountered inefficiencies using the predefined waste categories. This helps identify patterns.
5.  **Update Statuses:** As you progress, update the status of Topics (`In Progress`, `Needs Review`, `Completed`, etc.).
6.  **Monitor Progress:** Check Module progress bars, total time per topic, and your daily Study Streak on the dashboard or relevant views. Analyze waste patterns to improve future study sessions.

## 🤝 Contributing

Contributions are welcome! If you have suggestions for improvements or encounter issues, please feel free to open an issue or submit a pull request.

Please adhere to standard coding practices and ensure any contributions align with the project's goals. For major changes, please open an issue first to discuss what you would like to change.


## 📄 License

Distributed under the MIT License. See `LICENSE` file for more information.

## 📧 Contact

Adrian Grzybek - adrian.grzybek00@gmail.com

Project Link: [https://github.com/xkarox/Kizuku](https://github.com/your_username_/Kizuku)

## 🙏 Acknowledgements

*   [List any resources, libraries, or individuals you want to thank here]


