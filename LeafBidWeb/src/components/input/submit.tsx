import s from './input.module.css';

interface SubmitProps {
    placeholder?: string;
}

export default function Submit({ placeholder }: SubmitProps) {
    return (
        <input type={"submit"} className={`${s.input} ${s.submit}`} value={placeholder} />
    )
}