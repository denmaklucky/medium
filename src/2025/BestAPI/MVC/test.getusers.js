import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    vus: 10,
    iterations: 1000,
};

const BASE_URL = 'https://localhost:7076/api/v1/home';

export default function () {
    const res = http.get(BASE_URL);

    check(res, {
        'GET status is 2xx or 3xx': (r) => r.status >= 200 && r.status < 400,
    });

    sleep(1);
}
