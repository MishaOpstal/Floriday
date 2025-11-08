"use client";

import { Form } from "react-bootstrap";
import s from './selectField.module.css';
import React from "react";

const DEFAULT_ORDER = 'X';
let counter = 1;

interface SelectFieldProps {
    jsonData: Record<string, { count: string }>;
}

// Zoekfunctie: filtert items op basis van de invoer door de stijl.display aan te passen
const search = (event: React.ChangeEvent<HTMLInputElement>) => {
        const filter = event.target.value.toLowerCase();
        const productList = document.querySelector(`.${s.productList}`);

        if (productList) {
            const items = productList.getElementsByClassName(s.productItem);

            // Loop door alle items en verberg/toon ze op basis van filter
            for (let i = 0; i < items.length; i++) {
                const item = items[i] as HTMLElement;
                const textValue = item.textContent || item.innerText;

                if (textValue.toLowerCase().indexOf(filter) > -1) {
                    item.style.display = "";
                } else {
                    item.style.display = "none";
                }
            }
    }
}

// Handler voor klik op item: wijzigt de order nummer of zet het terug op X
const ChangeOrder = (event: React.MouseEvent<HTMLDivElement>) => {
    const item = event.currentTarget as HTMLElement;
    const orderSpan = item.querySelector(`.${s.order}`) as HTMLElement;

    // Als order is X (niet geselecteerd): voeg toe met counter
    // Als order is getal (wel geselecteerd): zet terug op X
    if (orderSpan.innerText === DEFAULT_ORDER) {
        search ({target: {value: ''}} as React.ChangeEvent<HTMLInputElement>);
        orderSpan.innerText = counter.toString();
        counter++;
    } else {
        orderSpan.innerText = DEFAULT_ORDER;
        // TODO: Hier moet de counter worden aangepast voor items erna (decrementeren)
    }
}

export default function SelectField({jsonData}: SelectFieldProps) {
    return (
        <div className={s.selectContainer}>
            <Form.Control
                type="text"
                placeholder="Zoeken..."
                className={s.selectField}
                onChange={search}
            />
            <hr className={s.divider}/>
            <div className={s.productList} role="listbox">
                {Object.keys(jsonData).length > 0 ? (
                    Object.entries(jsonData).map(([key, value]) => {
                        // OPMERKING: isSelected is altijd false - dit zou via state moeten
                        const isSelected = false;
                        return (
                            <div
                                key={key}
                                className={s.productItem}
                                id={key}
                                role="option"
                                aria-selected={isSelected}
                                onClick={ChangeOrder}
                                style={{ cursor: 'pointer', opacity: isSelected ? 0.6 : 1 }}
                            >
                                <span className={s.order}>X</span>
                                <span>
                                    {key}
                                </span>
                                <span className={s.productCount}>{value.count}</span>
                            </div>
                        );
                    })
                ) : (
                    <span className={s.noProducts}>Geen producten gevonden</span>
                )}
            </div>
        </div>

    )
}