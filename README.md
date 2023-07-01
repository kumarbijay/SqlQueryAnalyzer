# SQL Query Analyzer

SQL Query Analyzer is a simple Windows application that analyzes SQL queries and adds the necessary NOLOCK hints to improve query performance. It allows you to enter SQL queries, analyze them, and view the modified queries with the added NOLOCK hints. It also provides analysis for the creation and dropping of temp tables.

## Features

- Add NOLOCK hints to SELECT statements in the FROM and JOIN clauses.
- Analyze the creation and dropping of temp tables.
- Highlights NOLOCK hints, temp table creation, and temp table dropping in the modified queries.
- Copy the modified queries to the clipboard.
- Responsive user interface that adjusts to window resizing.

## Getting Started

### Prerequisites

- Windows operating system
- .NET Framework

### Installation

1. Download the latest setup file from the [Setup](https://github.com/kumarbijay/SqlQueryAnalyzer/Setup) page.
2. Install the tool to a desired location on your computer.

### Usage

1. Double-click on the "SqlQueryAnalyzer.exe" file to launch the application.
2. Enter your SQL query in the provided text area.
3. Click on the "Analyze NOLOCKS" button to analyze the query and add NOLOCK hints.
4. The modified query with added NOLOCK hints will be displayed in the result text area.
5. The application will also analyze the creation and dropping of temp tables and highlight them accordingly.
6. You can copy the modified query to the clipboard by clicking the "Copy Output" button.

## Contributing

Contributions are welcome! If you have any ideas, improvements, or bug fixes, please open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
