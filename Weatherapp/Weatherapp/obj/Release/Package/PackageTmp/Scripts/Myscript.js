(function ($) {
    "use strict";
    var myNewChart;
    $(document).ready(function () {
        // Get the context of the canvas element we want to select
        var ctx = document.getElementById("myChart").getContext("2d");

        var data = {
            labels: ["00"],//, "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24"],
            datasets: [
                {
                    label: "My First dataset",
                    fillColor: "rgba(220,220,220,0.2)",
                    strokeColor: "rgba(220,220,220,1)",
                    pointColor: "rgba(220,220,220,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(220,220,220,1)",
                    data: [20]
                },
                {
                    label: "My Second dataset",
                    fillColor: "rgba(151,187,205,0.2)",
                    strokeColor: "rgba(151,187,205,1)",
                    pointColor: "rgba(151,187,205,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(151,187,205,1)",
                    data: [28]
                }
            ]
        };
        myNewChart = new Chart(ctx).Line(data, {
            bezierCurve: false
        });

        Chart.defaults.global.responsive = true;

        
    });

    $("#myButton").click(function addData() {
        var datetime = new Date();
        var a = $("#myNumber").val();
        console.log(a);
        if (myNewChart.eachPoints.length > 10){
            myNewChart.removeData();
        }

        myNewChart.addData([a], datetime.getHours());
    });

    // set up the timeout variable
    var t;
    // setup the sizing function,
    // with an argument that tells the chart to animate or not
    function size(animate){
        // If we are resizing, we don't want the charts drawing on every resize event.
        // This clears the timeout so that we only run the sizing function
        // when we are done resizing the window
        clearTimeout(t);
        // This will reset the timeout right after clearing it.
        t = setTimeout(function(){
            $("canvas").each(function(i,el){
                // Set the canvas element's height and width to it's parent's height and width.
                // The parent element is the div.canvas-container
                $(el).attr({
                    "width":$(el).parent().width(),
                    "height":$(el).parent().outerHeight()
                });
            });
            // kickoff the redraw function, which builds all of the charts.
            redraw(animate);

            // loop through the widgets and find the tallest one, and set all of them to that height.
            var m = 0;
            // we have to remove any inline height setting first so that we get the automatic height.
            $(".widget").height("");
            $(".widget").each(function(i,el){ m = Math.max(m,$(el).height()); });
            $(".widget").height(m);

        }, 100); // the timeout should run after 100 milliseconds
    }
    $(window).on('resize', size);
    function redraw(animation){
        var options = {};
        if (!animation){
            options.animation = false;
        } else {
            options.animation = true;
        }
        // ....
        // the rest of our chart drawing will happen here
        // ....
    }
    
    

    var data2 = {
        labels : ["Mon","Tue","Wed","Thu","Fri","Sat","Sun"],
        datasets : [
            {
                fillColor : "rgba(99,123,133,0.4)",
                strokeColor : "rgba(220,220,220,1)",
                pointColor : "rgba(220,220,220,1)",
                pointStrokeColor : "#fff",
                data : [65,54,30,81,56,55,40]
            },
            {
                fillColor : "rgba(219,186,52,0.4)",
                strokeColor : "rgba(220,220,220,1)",
                pointColor : "rgba(220,220,220,1)",
                pointStrokeColor : "#fff",
                data : [20,60,42,58,31,21,50]
            },
        ]
    }
    var canvas = document.getElementById("indoorTemp");
    var ctx = canvas.getContext("2d");
    new Chart(ctx).Line(data2);
})(jQuery);

