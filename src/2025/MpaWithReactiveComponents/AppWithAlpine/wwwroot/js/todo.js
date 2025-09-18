function xsrf() {
  const el = document.querySelector('meta[name="request-verification-token"]');
  return el?.content ?? '';
}

async function jfetch(url, init) {
  const res = await fetch(url, {
    credentials: 'same-origin',
    headers: { 'Content-Type': 'application/json', ...(init?.headers || {}) },
    ...init
  });
  if (!res.ok) throw new Error(`${res.status} ${res.statusText}`);
  return res.json();
}

document.addEventListener('alpine:init', () => {
  Alpine.data('tasks', () => ({
    incompletedItems: [],
    completedItems: [],  
    title: '',
    error: '',

    async load() {
      try {
        this.error = '';
        this.incompletedItems = await jfetch('/todo?handler=ListIncompleted');
        this.completedItems = await jfetch('/todo?handler=ListCompleted');
      } catch (e) {
        this.error = `Failed to load: ${e}`;
      }
    },

    async add() {
      const t = (this.title || '').trim();
      if (!t) return;
      try {
        this.error = '';
        await jfetch('/todo?handler=Add', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': xsrf() },
          body: JSON.stringify({ title: t })
        });
        this.title = '';
        await this.load();
      } catch (e) {
        this.error = `Add failed: ${e}`;
      }
    },

    async toggle(task) {
      try {
        this.error = '';
        await jfetch('/todo?handler=Update', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': xsrf() },
          body: JSON.stringify({ id: task.id, title: task.title, isCompleted: !task.isCompleted })
        });
        await this.load();
      } catch (e) {
        this.error = `Update failed: ${e}`;
      }
    },

    async remove(id) {
      try {
        this.error = '';
        await jfetch('/todo?handler=Delete', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': xsrf() },
          body: JSON.stringify({ id })
        });
        await this.load();
      } catch (e) {
        this.error = `Delete failed: ${e}`;
      }
    },
  }));
});
