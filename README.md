# InfluxDB Project

This project consists of two applications:
1. **Test Application (Data Writer)**: Writes data to InfluxDB.
2. **Webapp Application (Data Visualization)**: Visualizes the data from InfluxDB in the form of graphs.

## Prerequisites

- Ensure that you have **InfluxDB Cloud** set up with the correct credentials.
- Ensure you have **.NET SDK** installed on your machine. You can download it from the official website: [dotnet.microsoft.com](https://dotnet.microsoft.com/download).

---

## Running the Test Application (Data Writer)

### Steps:

1. **Ensure InfluxDB Cloud is properly configured** with the correct credentials.
2. Open a terminal and navigate to the `test` folder:
    ```bash
    cd path/to/test
    ```
3. Run the application using the following command:
    ```bash
    dotnet run
    ```

This application will write data to your InfluxDB database.

---

## Running the Webapp Application (Data Visualization)

### Steps:

1. Open a terminal and navigate to the `webapp` folder:
    ```bash
    cd path/to/webapp
    ```
2. Run the application using the following command:
    ```bash
    dotnet run
    ```
3. Once the application is running, open a browser and go to the following URL to view the data visualizations:
    ```
    http://localhost:5080
    ```

The webapp will display the data stored in InfluxDB as graphs.

---

## Troubleshooting

- If you encounter any issues with running the applications, ensure that:
  - Your InfluxDB credentials are correct.
  - The applications are using the correct URLs and ports.
  - You have a stable internet connection for accessing InfluxDB Cloud.
- For further assistance, please contact [support@influxdata.com](mailto:support@influxdata.com).
