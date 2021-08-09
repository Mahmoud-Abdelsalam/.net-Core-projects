var ii = 0;
var sum = 0;
var rowGlobal = $(".row-0")[0].outerHTML;
var str = '';

str += `<tr class="row-0">
                                <td>

                                    <div class="form-group">
                                        <input name="KPIDetails[0].KPIDNum" value="" type="number" data-valmsg-for="KPIDetails[0].KPIDNum" data-valmsg-replace="true" class="form-control" required />
                                        <span class="text-danger field-validation-valid" data-valmsg-for="KPIDetails[0].KPIDNum" data-valmsg-replace="true"></span>
                                    </div>
                                </td>
                                <td>
                                    <div class="form-group">
                                        <input name="KPIDetails[0].KPIDDescription" value="" class="form-control" data-valmsg-for="KPIDetails[0].KPIDDescription" data-valmsg-replace="true" type="text"  class="form-control" required/>
                                          <span class="text-danger field-validation-valid" data-valmsg-for="KPIDetails[0].KPIDDescription" data-valmsg-replace="true"></span>

                                    </div>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <div class="form-group">
                                        <input id="targetvalue-0"  type="number" value="" class="form-control" data-valmsg-for="KPIDetails[0].TargetValue" data-valmsg-replace="true" name="KPIDetails[0].TargetValue" onkeyup="CalcTotal();" class="form-control " required />
                                        <span class="text-danger field-validation-valid" data-valmsg-for="KPIDetails[0].TargetValue" data-valmsg-replace="true"></span>
                                    </div>
                                </td>
                            </tr>`;
function reloadPage() {
    location.reload(true);
};

$(document).ready(function () {
    $(".measure").on('change', function () {
        $(".measure").not(this).prop('checked', false);
    });
});

$('#perc').on("change",
    function () {
        if ($('#perc').is(":checked")) {
            $("#percSpan").show();
        } else {
            $("#percSpan").hide();
        }
    });

$('#num').on("change",
    function () {
        if ($('#num').is(":checked")) {
            $("#percSpan").hide();
        }
    });
$('#remove').on("click",
    function () {
        $('tbody tr:last').remove();
        ii -= 1;
    });

function CalcTotal() {
    sum = 0;
    var sum = $("input[id^='targetvalue'][type='number']")
        .get()
        .reduce((res, elm) =>
            (res + (+elm.value || 0)), 0);
    if ($('#perc').is(":checked") === true && sum > 100) {
        $("#max").css("display", "block");
        $("#submit").hide();
    } else {
        $("#max").css("display", "none");
        $("#submit").show();
    };
    $('.total').val(sum);
};

$("#add-btn").click(function () {
    ii += 1;
    var row = str;
    row = row.replace(/\[[0]\]/g, "[" + ii + "]");
    row = row.replace(/\-[0]/g, "-" + ii);
    $("#table_body").append(row);
    //$('table tbody tr:last td div input').val("");

    console.log(row);
});

$('#New').on("click",
    function () {
        var row = rowGlobal;
        ii = 0;
        $('tbody').children('tr').remove();

        row = row.replace(/\[[0]\]/g, "[" + ii + "]");
        row = row.replace(/\-[0]/g, "-" + ii);

        $(".record-list").append(row);

        $(".measure").prop('checked', false);

        $('#myform')[0].reset();
        $('#DepNo').val(dep);
        $("#max").css("display", "none");
        $("#submit").show();
    });

async function GetRows(id) {
    var Dep = id;
    if (Dep != "") {
        var rows = await axios.get(`/KPI/GetDepartmentKPIs?department=${Dep}`);
        if (rows.data != "") {
            if (rows.data[0].measurmentUnit == true) {
                $("#perc").prop('checked', true);
                $("#num").prop('checked', false);
                $("#percSpan").show();
            } else {
                $("#num").prop('checked', true);
                $("#perc").prop('checked', false);
                $("#percSpan").hide();
            }
            renderTableRows(rows.data);
        } else {
            emptyTableRows();
        }
    } else {
        emptyTableRows();
    }
};
var renderTableRows = function (options) {
    ii = 0;
    var str = '';
    options.forEach(function (elem, i) {
        str += ` <tr class="row-${i}">
                                <td>

                                    <div class="form-group">
                                        <input name="KPIDetails[${i}].KPIDNum" type="number" data-valmsg-for="KPIDetails[${i}].KPIDNum" data-valmsg-replace="true"  value="${elem.kpidNum}" class="form-control" required />
                                        <span class="text-danger field-validation-valid" data-valmsg-for="KPIDetails[${i}].KPIDNum" data-valmsg-replace="true"></span>
                                    </div>
                                </td>
                                <td>
                                    <div class="form-group">
                                        <input name="KPIDetails[${i}].KPIDDescription"  data-valmsg-for="KPIDetails[${i}].KPIDDescription" data-valmsg-replace="true" type="text" value="${elem.kpidDescription}" class="form-control" required/>
                                        <span class="text-danger field-validation-valid" data-valmsg-for="KPIDetails[${i}].KPIDDescription" data-valmsg-replace="true"></span>

                                    </div>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <div class="form-group">
                                        <input id="targetvalue-${i}"  type="number"  data-valmsg-for="KPIDetails[${i}].TargetValue" data-valmsg-replace="true" name="KPIDetails[${i}].TargetValue" value="${elem.targetValue}" onkeyup="CalcTotal();" class="form-control " required />
                                        <span class="text-danger field-validation-valid" data-valmsg-for="KPIDetails[${i}].TargetValue" data-valmsg-replace="true"></span>
                                    </div>
                                </td>
                            </tr>`;
        ii = i;
    });
    $('#table_body').empty().append(str);
};
var emptyTableRows = function () {
    var str = '';

    str += `<tr class="row-0">
                                <td>

                                    <div class="form-group">
                                        <input name="KPIDetails[0].KPIDNum" value="" type="number" data-valmsg-for="KPIDetails[0].KPIDNum" data-valmsg-replace="true" class="form-control" required />
                                        <span class="text-danger field-validation-valid" data-valmsg-for="KPIDetails[0].KPIDNum" data-valmsg-replace="true"></span>
                                    </div>
                                </td>
                                <td>
                                    <div class="form-group">
                                        <input name="KPIDetails[0].KPIDDescription"value="" data-valmsg-for="KPIDetails[0].KPIDDescription" data-valmsg-replace="true" type="text"  class="form-control" required/>
                                          <span class="text-danger field-validation-valid" data-valmsg-for="KPIDetails[0].KPIDDescription" data-valmsg-replace="true"></span>

                                    </div>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <div class="form-group">
                                        <input id="targetvalue-0"  type="number" value="" data-valmsg-for="KPIDetails[0].TargetValue" data-valmsg-replace="true" name="KPIDetails[0].TargetValue" onkeyup="CalcTotal();" class="form-control " required />
                                        <span class="text-danger field-validation-valid" data-valmsg-for="KPIDetails[0].TargetValue" data-valmsg-replace="true"></span>
                                    </div>
                                </td>
                            </tr>`;
    ii = 0;
    $('#table_body').empty().append(str);
};