function xsrf() {
  const el = document.querySelector<HTMLMetaElement>(
    'meta[name="request-verification-token"]'
  );
  return el?.content ?? '';
}

async function json<T>(url: string, options?: RequestInit): Promise<T> {
  const res = await fetch(url, {
    headers: { 'Content-Type': 'application/json', ...(options?.headers || {}) },
    credentials: 'same-origin',  // Razor cookie + XSRF cookie
    ...options
  });
  if (!res.ok) throw new Error(`${res.status} ${res.statusText}`);
  return res.json();
}

export type TaskEntity = {
  id: number;
  title: string | null;
  isCompleted: boolean;
  createdAt: string;
  updatedAt?: string | null;
};

export const TasksApi = {
  list: () => json<TaskEntity[]>('/todo?handler=List'),

  add: (title: string | null) =>
    json<TaskEntity>('/todo?handler=Add', {
      method: 'POST',
      body: JSON.stringify({ title }),
      headers: {
          'Content-Type': 'application/json',
          'RequestVerificationToken': xsrf() 
      }
    }),

  update: (t: Pick<TaskEntity, 'id' | 'title' | 'isCompleted'>) =>
    json<{ ok: boolean }>('/todo?handler=Update', {
      method: 'POST',
      body: JSON.stringify(t),
      headers: { 'RequestVerificationToken': xsrf() }
    }),

  remove: (id: number) =>
    json<{ ok: boolean }>('/todo?handler=Delete', {
      method: 'POST',
      body: JSON.stringify({ id }),
      headers: { 'RequestVerificationToken': xsrf() }
    }),
};
