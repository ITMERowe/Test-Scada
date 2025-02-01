async function fetchCpuUsage() {
    const response = await fetch('/Influx/CpuUsage');
    const data = await response.json();

    const tableBody = document.getElementById('cpuUsageTableBody');
    tableBody.innerHTML = '';

    data.forEach(row => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${row.host}</td>
            <td>${row.usage}</td>
            <td>${row.cores_active}</td>
            <td>${row.temperature}</td>
            <td>${row.time}</td>
        `;
        tableBody.appendChild(tr);
    });
}

// Fetch data on page load
window.onload = fetchCpuUsage;
