changeInputType();
$("#searchOptions").change(changeInputType);

function changeInputType() {
    optionNumber = $("#searchOptions").val();
    switch (optionNumber) {
        case ("0"):
            $("#inputLogEntriesSearch").attr('type', 'text');
            break;
        case ("1"):
            $("#inputLogEntriesSearch").attr('type', 'date');
            break;
        default:
            alert("Opción no habilitada, por favor, informe a sistemas. Search Options js Switch Audit Log");
            break;
    }
}