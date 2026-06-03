import { useCallback, useEffect, useState } from "react";

export const Counter = function () {
    // const obj = useState(0);
    // const count = obj[0];
    // const setCount = obj[1];
    const [count, setCount] = useState(0);

    const handleClick = useCallback(() => {
        setCount(oldValue => oldValue + 1);
    }, []);

    useEffect(() => {
        console.log("Counter created");
    }, []);

    return (
        <div className="counter">
            Counter will be here {count}
            <button type="button" onClick={handleClick}>+</button>
        </div>
    );
}
