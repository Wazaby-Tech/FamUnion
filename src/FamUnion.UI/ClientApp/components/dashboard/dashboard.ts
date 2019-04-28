import Vue from 'vue';
import { Component } from 'vue-property-decorator';

interface Reunion {
    name: string;
    description: string;
    startDate: Date;
    endDate: Date;
    cityLocation: object;
}

@Component
export default class DashboardComponent extends Vue {
    reunions: Reunion[] = [];

    mounted() {
        fetch('https://localhost:44313/api/reunions/list')
            .then(response => response.json() as Promise<Reunion[]>)
            .then(data => {
                this.reunions = data;
            });
    }
}
