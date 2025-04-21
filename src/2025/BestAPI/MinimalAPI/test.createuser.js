import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    vus: 10,
    iterations: 1000,
};

const BASE_URL = 'https://localhost:7179/api/v1/home';

export default function () {
    const payload = JSON.stringify({ email: 'test@mail.com' });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const res = http.post(BASE_URL, payload, params);

    check(res, {
        'POST status is 2xx or 3xx': (r) => r.status >= 200 && r.status < 400,
    });

    sleep(1);
}
