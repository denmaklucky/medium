import {useEffect, useState} from 'react'
import {TasksApi, type TaskEntity} from './api'

export default function App() {
    const [incompletedItems, setIncompletedItems] = useState<TaskEntity[]>([])
    const [completedItems, setCompletedItems] = useState<TaskEntity[]>([])
    const [title, setTitle] = useState('')

    async function load() {
        const completedItems = await TasksApi.listCompleted()
        setCompletedItems(completedItems)

        const incompletedItems = await TasksApi.listIncompleted();
        setIncompletedItems(incompletedItems)
    }

    useEffect(() => {
        load()
    }, [])

    async function add() {
        if (!title.trim()) return
        await TasksApi.add(title.trim())
        setTitle('')
        await load()
    }

    async function completed(id: number) {
        const t = incompletedItems.find(x => x.id === id)
        if (!t) return
        await TasksApi.update({id, title: t.title, isCompleted: true})
        await load()
    }

    async function incompleted(id: number) {
        const t = completedItems.find(x => x.id === id)
        if (!t) return
        await TasksApi.update({id, title: t.title, isCompleted: false})
        await load()
    }

    async function remove(id: number) {
        await TasksApi.remove(id)
        await load()
    }

    return (
        <div className="p-4">
            <div style={{display: 'flex', gap: 8}}>
                <input className="form-control"
                       value={title}
                       onChange={e => setTitle(e.target.value)}
                       placeholder="New task title..."/>
                <button type="button" className="btn btn-primary" onClick={add}>Add</button>
            </div>

            <h5 className="m-3">Incompleted task</h5>

            <ul className="container">
                {incompletedItems.map(t => (
                    <li key={t.id}>
                        <div className="card">
                            <div className="card-body task">
                                <input
                                    className="form-check-input"
                                    type="checkbox"
                                    checked={t.isCompleted}
                                    onChange={_ => completed(t.id)}
                                />

                                <span>
                                    {t.title ?? '(no title)'}
                                </span>

                                <button type="button" className="btn btn-danger" onClick={() => remove(t.id)}>Delete</button>
                            </div>
                        </div>
                    </li>
                ))}
            </ul>

            <h5 className="m-3">Completed task</h5>

            <ul className="container">
                {completedItems.map(t => (
                    <li key={t.id}>
                        <div className="card">
                            <div className="card-body task">
                                <input
                                    className="form-check-input"
                                    type="checkbox"
                                    checked={t.isCompleted}
                                    onChange={_ => incompleted(t.id)}
                                />

                                <span style={{textDecoration: 'line-through'}}>
                                    {t.title ?? '(no title)'}
                                </span>

                                <button type="button" className="btn btn-danger" onClick={() => remove(t.id)}>Delete</button>
                            </div>
                        </div>
                    </li>
                ))}
            </ul>

        </div>
    )
}
