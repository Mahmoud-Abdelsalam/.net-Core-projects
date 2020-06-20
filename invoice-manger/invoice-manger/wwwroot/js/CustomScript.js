var price, qty, discount, total, net, totalInvoice, totalTaxes, totalNet, newHtml, html;
var i = 0;
var Nets = 0;
function Calculate(i) {
    price = document.getElementById("price-" + i).value;
    qty = document.getElementById("qty-"+i).value;
    discount = document.getElementById("discount-"+ i).value;
    total = document.getElementById("total-" +i).value = price * qty;
    net = document.getElementById("net-" +i).value = total - (total * (discount / 100));
    return net;
}

document.getElementById("total-taxes").addEventListener("focus", function () {
    Nets = Nets + net; 
    totalInvoice = document.getElementById("total-invoice").value = Nets;
});

document.getElementById("total-taxes").addEventListener("keyup", function () {
    totalTaxes = document.getElementById("total-taxes").value;
    totalNet = document.getElementById("total-net").value = totalInvoice + (totalInvoice * (totalTaxes / 100));
});

document.getElementById("add-btn").addEventListener("click", function() {
    totalTaxes = document.getElementById("total-taxes").value = 0;
});

$("#add-btn").click(function() {
    i += 1;
    var row = $(".row-0")[0].outerHTML;
    row = row.replace(/\[[0]\]/g, "[" + i + "]");
    row = row.replace(/\-[0]/g, "-" + i);
    row = row.replace(/\([0]\)/g, "(" + i + ")");
    $(".record-list").append(row);
    console.log(row);
});

