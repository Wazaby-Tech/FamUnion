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
    loading: boolean = true;

    mounted() {
        fetch('https://localhost:44397/api/reunions/list')
            .then(response => response.json() as Promise<Reunion[]>)
            .then(data => {
                this.reunions = data;
            })
            .then(() => {
                this.loading = false;
            });
    }

    //deleteClick() {
    //    fetch("https://localhost:44397/api/reunions/delete",
    //        {
    //            method: 'post',
    //        })

         
    //}
}
