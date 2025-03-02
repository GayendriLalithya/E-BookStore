// Base class for abstraction and polymorphism
class TableManager {
    constructor(tableId) {
        this.table = document.getElementById(tableId);
        if (!this.table) {
            throw new Error(`Table with ID '${tableId}' not found.`);
        }
    }

    // Abstract method for sorting
    sortTable(columnIndex, dir = "asc") {
        throw new Error("sortTable method must be implemented by a subclass.");
    }

    // Abstract method for generating reports
    generateReport() {
        throw new Error("generateReport method must be implemented by a subclass.");
    }
}

// Subclass for managing CSV reports
class CsvTableManager extends TableManager {
    sortTable(columnIndex, dir = "asc") {
        const rows = Array.from(this.table.querySelectorAll("tbody tr"));
        const sortedRows = rows.sort((a, b) => {
            const aText = a.cells[columnIndex].innerText.trim();
            const bText = b.cells[columnIndex].innerText.trim();
            return dir === "asc" ? aText.localeCompare(bText) : bText.localeCompare(aText);
        });

        sortedRows.forEach(row => this.table.querySelector("tbody").appendChild(row));
    }

    generateReport(filename = "report.csv") {
        const rows = Array.from(this.table.querySelectorAll("tr"));
        const csvContent = rows
            .map(row =>
                Array.from(row.querySelectorAll("td, th"))
                    .map(cell => cell.innerText)
                    .join(",")
            )
            .join("\n");
        const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = filename;
        link.click();
    }
}

// Subclass for managing PDF reports
class PdfTableManager extends TableManager {
    sortTable(columnIndex, dir = "asc") {
        const rows = Array.from(this.table.querySelectorAll("tbody tr"));
        const sortedRows = rows.sort((a, b) => {
            const aText = a.cells[columnIndex].innerText.trim();
            const bText = b.cells[columnIndex].innerText.trim();
            return dir === "asc" ? aText.localeCompare(bText) : bText.localeCompare(aText);
        });

        sortedRows.forEach(row => this.table.querySelector("tbody").appendChild(row));
    }

    async generateReport(filename = "report.pdf") {
        const { jsPDF } = window.jspdf;
        const doc = new jsPDF();
        const rows = Array.from(this.table.querySelectorAll("tr"));
        const data = rows.map(row =>
            Array.from(row.querySelectorAll("td, th")).map(cell => cell.innerText)
        );

        doc.autoTable({
            head: [data[0]], // Use the first row as the header
            body: data.slice(1), // Remaining rows as body
            theme: "striped",
            headStyles: { fillColor: [40, 40, 40] },
            styles: { halign: "center" }
        });

        doc.save(filename);
    }
}
