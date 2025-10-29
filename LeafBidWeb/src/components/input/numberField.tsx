import s from './input.module.css';

interface NumberFieldProps {
    placeholder: string;
    step?: number;
    value?: string | number;
    onChange?: (value: string) => void;
}

export default function NumberField({ placeholder, step = 1, value, onChange }: NumberFieldProps) {
    return (
            <input
                type="number"
                className={s.input}
                placeholder={placeholder}
                step={step}
                value={value ?? ''}
                onChange={(e) => onChange?.(e.target.value)}
            />
    )
}