# CloudQA Automation Assessment 🚀

Welcome! This repository contains my solution for the CloudQA automation assessment.
It features robust, resilient automated tests for the [Automation Practice Form](https://app.cloudqa.io/home/AutomationPracticeForm).

## 🎯 The Goal
The main objective was to write tests for 3 fields (**First Name**, **Last Name**, **Email**) that **don't break** even if the underlying HTML attributes or positions change.

## 🛠️ Tech Stack
*   **Language**: C# (.NET 8.0)
*   **Framework**: NUnit
*   **Driver**: Selenium WebDriver (Chrome)

## 💡 Key Feature: Robust Selectors
Instead of relying on brittle IDs or XPaths, I implemented a "human-like" selector strategy (`GetInputByLabel`).
It finds fields by their **visible label text** using 4 fallback strategies:

1.  **Semantic**: Matches `label for="id"` ➡️ `input id="..."` (Best Practice)
2.  **Nesting**: Checks if the input is *inside* the label tag.
3.  **Sibling**: Checks if the input is the *immediate neighbor* of the label.
4.  **Proximity**: Checks the parent container for a matching input.

This ensures the tests behave like a real user: as long as the label "First Name" is visible near a box, the test will find it!

## 🏃‍♂️ How to Run

### Prerequisites
*   [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download) installed.

### Steps
1.  **Clone the repository**:
    ```bash
    git clone https://github.com/deshmukh3421/CloudQA_AitomationTesting.git
    ```
2.  **Navigate to the project**:
    ```bash
    cd AutomationTests
    ```
3.  **Run the tests**:
    ```bash
    dotnet test
    ```

## 📝 Test Coverage
*   **`Test_FillFormFields_Robustly`**:
    *   Opens the browser.
    *   Locates fields by label text.
    *   Fills them with test data.
    *   Verifies the data was entered correctly.
