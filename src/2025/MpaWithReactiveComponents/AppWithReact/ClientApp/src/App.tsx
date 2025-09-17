import { useEffect, useState } from 'react'
import { TasksApi, type TaskEntity } from './api'

export default function App() {
  const [items, setItems] = useState<TaskEntity[]>([])
  const [title, setTitle] = useState('')

  async function load() {
    const list = await TasksApi.list()
    setItems(list)
  }

  useEffect(() => { load() }, [])

  async function add() {
    if (!title.trim()) return
    await TasksApi.add(title.trim())
    setTitle('')
    await load()
  }

  async function toggle(id: number, isCompleted: boolean) {
    const t = items.find(x => x.id === id)
    if (!t) return
    await TasksApi.update({ id, title: t.title, isCompleted })
    await load()
  }

  async function remove(id: number) {
    await TasksApi.remove(id)
    await load()
  }

  return (
    <div className="p-4">
      <div style={{ display:'flex', gap:8 }}>
        <input className="form-control"
          value={title}
          onChange={e => setTitle(e.target.value)}
          placeholder="New task title..."/>
        <button type="button" className="btn btn-primary" onClick={add}>Add</button>
      </div>

      <ul>
        {items.map(t => (
          <li key={t.id} style={{ display:'flex', gap:8, alignItems:'center' }}>
            <input
              type="checkbox"
              checked={t.isCompleted}
              onChange={e => toggle(t.id, e.target.checked)}
            />
            <span style={{ textDecoration: t.isCompleted ? 'line-through' : 'none' }}>
              {t.title ?? '(no title)'}
            </span>
            <button onClick={() => remove(t.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  )
}
