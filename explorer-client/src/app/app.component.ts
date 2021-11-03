import { Component } from '@angular/core';
import * as Highcharts from 'highcharts';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'explorer-client';

  transCharthighcharts = Highcharts;
  tokensCharthighcharts = Highcharts;
  usersCharthighcharts = Highcharts;

  transchartOptions: Highcharts.Options = {
    title: {
      text: "Average Transactions",
    },
    xAxis: {
      title: {
        text: 'Months'
      },
      categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
    },
    yAxis: {
      title: {
        text: "Transactions"
      }
    },
    credits: {
      enabled: false
    },
    series: [{
      name: 'Count',
      data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 24.4, 19.3, 16.0, 18.4, 17.9],
      type: 'line',
      showInLegend: false,
    }]
  }

  tokenschartOptions: Highcharts.Options = {
    title: {
      text: "Average Tokens"
    },
    xAxis: {
      title: {
        text: 'Months'
      },
      categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
    },
    yAxis: {
      title: {
        text: "Tokens"
      }
    },
    credits: {
      enabled: false
    },
    series: [{
      name: 'Count',
      data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 24.4, 19.3, 16.0, 18.4, 17.9],
      type: 'column',
      showInLegend: false,
    }]
  }
  
  userschartOptions: Highcharts.Options = {
    title: {
      text: "Average Tokens"
    },
    xAxis: {
      title: {
        text: 'Months'
      },
      categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
    },
    yAxis: {
      title: {
        text: "Tokens"
      }
    },
    credits: {
      enabled: false
    },
    series: [{
      name: 'Count',
      data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 24.4, 19.3, 16.0, 18.4, 17.9],
      type: 'pie',
      showInLegend: false,
    }]
  }
}
