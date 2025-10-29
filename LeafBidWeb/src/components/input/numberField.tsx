import s from './input.module.css';

interface NumberFieldProps {
    placeholder: string;
    step?: number;
}

export default function NumberField({ placeholder, step = 1 }: NumberFieldProps) {
    return (
            <input type="number" className={s.input} placeholder={placeholder} step={step} />
    )
}