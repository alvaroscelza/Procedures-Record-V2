changeInputType();
$("#searchOptions").change(changeInputType);

function changeInputType() {
    optionNumber = $("#searchOptions").val();
    switch (optionNumber) {
        case ("0"):
            $("#inputPeopleSearch").attr('type', 'text');
            break;
        case ("1"):
            $("#inputPeopleSearch").attr('type', 'number');
            break;
        case ("2"):
            $("#inputPeopleSearch").attr('type', 'number');
            break;
        case ("3"):
            $("#inputPeopleSearch").attr('type', 'date');
            break;
        case ("4"):
            $("#inputPeopleSearch").attr('type', 'text');
            break;
        default:
            alert("Opción no habilitada, por favor, informe a sistemas. Search Options js Switch People");
            break;
    }
}